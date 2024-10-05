using ADDESAPI.Core.Asignacion;
using ADDESAPI.Core.Asignacion.DTO;
using ADDESAPI.Core.AsignacionCQRS.DTO;
using ADDESAPI.Core.Colaborador;
using ADDESAPI.Core.EstacionCQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.AsignacionCQRS
{
    public class AsignacionService : IAsignacionService
    {
        private readonly IAsignacionResource _resource;
        private readonly IColaboradorResource _resourceColaborador;
        private readonly IEstacionResource _resourceEstacion;

        public AsignacionService(IAsignacionResource resource, IColaboradorResource resourceColaborador, IEstacionResource resourceEstacion)
        {
            _resource = resource;
            _resourceColaborador = resourceColaborador;
            _resourceEstacion = resourceEstacion;
        }
        public async Task<ResultSingle<AsignacionesDTO>> GetAsignaciones(GetAsignacionesReqDTO req)
        {
            ResultSingle<AsignacionesDTO> Result = new ResultSingle<AsignacionesDTO>();
            List<AsignacionColaboradorDTO> Asignaciones = new List<AsignacionColaboradorDTO>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Fecha", Value = req.Fecha});
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Password", Value = req.Turno.ToString() });
                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultColaboradores = await _resourceColaborador.GetColaboradores();
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultColaboradores.Success;
                    Result.Error = ResultColaboradores.Error;
                    Result.Message = ResultColaboradores.Message;
                    return Result;
                }
                var Colaboradores = ResultColaboradores.Data;

                var ResultAsignaciones = await _resource.GetAsignaciones(req.Fecha, req.Turno);
                if (ResultAsignaciones.Success)
                {
                    Asignaciones = ResultAsignaciones.Data.ToList();
                }

                Result.Success = true;
                Result.Error = "";
                Result.Message = "";
                Result.Data = new AsignacionesDTO();
                Result.Data.Colaboradores = Colaboradores.ToList();
                Result.Data.Asignaciones = Asignaciones.ToList();
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Excepcion";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<AsignacionColaboradorTurno>> GetAsignacion(GetAsignacionReqDTO req)
        {
            ResultSingle<AsignacionColaboradorTurno> Result = new ResultSingle<AsignacionColaboradorTurno>();
            AsignacionColaboradorTurno AsignacionTurno = new AsignacionColaboradorTurno();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "NoEmpleado", Value = req.NoEmpleado.ToString() });
                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultGasolinera = await _resourceEstacion.GetGasolinera();
                if (!ResultGasolinera.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultGasolinera.Error;
                    Result.Message = ResultGasolinera.Message;
                    return Result;
                }
                var Gasolinera = ResultGasolinera.Data;

                var ResultAsignacion = await _resource.GetAsignacion(req.NoEmpleado, Gasolinera.Fecha, Gasolinera.TurnoActual);
                if (!ResultAsignacion.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultAsignacion.Error;
                    Result.Message = ResultAsignacion.Message;
                    return Result;
                }
                var Asignacion = ResultAsignacion.Data;
                AsignacionTurno.Turno = Gasolinera.TurnoActual;
                AsignacionTurno.Asignacion = new List<AsignacionDTO>();
                AsignacionTurno.Asignacion = Asignacion.Select(x => new AsignacionDTO { Bomba = x.Bomba, IdBomba = x.IdBomba, NoBomba = x.NoBomba, noIsla = x.noIsla }).ToList();

                Result.Success = true;
                Result.Error = "";
                Result.Message = "Asignación encontrada";
                Result.Data = AsignacionTurno;
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Excepcion";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
