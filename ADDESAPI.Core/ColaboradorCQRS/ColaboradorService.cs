using ADDESAPI.Core.Asignacion;
using ADDESAPI.Core.Asignacion.DTO;
using ADDESAPI.Core.Colaborador.DTO;
using ADDESAPI.Core.EstacionCQRS;
using ADDESAPI.Core.ImpresoraCQRS;
using ADDESAPI.Core.ImpresoraCQRS.DTO;
using ADDESAPI.Core.ModuloCQRS;
using ADDESAPI.Core.VentukCQRS;
using Proteo5.HL.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.Colaborador
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly IColaboradorResource _resource;
        private readonly IVentukResource _resourceVentuk;
        private readonly IAsignacionResource _resourceAsignacion;
        private readonly IEstacionResource _resourceEstacion;
        private readonly IImpresoraResource _resourceImpresoras;
        private readonly IModuloResource _resourceModulo;
        public ColaboradorService(IColaboradorResource colaboradorResource, IVentukResource ventukResource, IAsignacionResource resourceAsignacion, IEstacionResource resourceEstacion, IImpresoraResource resourceImpresoras, IModuloResource resourceModulo)
        {
            _resource = colaboradorResource;
            _resourceVentuk = ventukResource;
            _resourceAsignacion = resourceAsignacion;
            _resourceEstacion = resourceEstacion;
            _resourceImpresoras = resourceImpresoras;
            _resourceModulo = resourceModulo;
        }

        public async Task<ResultSingle<ColaboradorDTO>> Login(RequestLoginDTO request)
        {
            ResultSingle<ColaboradorDTO> Result = new ResultSingle<ColaboradorDTO>();

            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Usuario", Value = request.Usuario });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Password", Value = request.Password });
                //valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Fecha", Value = request.Fecha });
                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultLogin = await _resource.Login(request.Usuario, request.Password);

                if (!ResultLogin.Success)
                {
                    Result.Success = ResultLogin.Success;
                    Result.Error = ResultLogin.Error;
                    Result.Message = ResultLogin.Message;
                    return Result;
                }
                ColaboradorDTO colaboradorDTO = new ColaboradorDTO();
                colaboradorDTO.NumeroVentuk = ResultLogin.Data.NumeroVentuk;
                colaboradorDTO.Nombre = ResultLogin.Data.Nombre;
                colaboradorDTO.Estatus = ResultLogin.Data.Estatus;
                colaboradorDTO.NoEstacion = ResultLogin.Data.NoEstacion;
                colaboradorDTO.Estacion = ResultLogin.Data.Estacion;
                colaboradorDTO.Puesto = ResultLogin.Data.Puesto;
                colaboradorDTO.ROL = ResultLogin.Data.ROL;
                colaboradorDTO.IdRol = ResultLogin.Data.IdRol;

                if (colaboradorDTO.Estatus != 1)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = "El usuario se encuentra dado de baja";
                    return Result;
                }

                
                var ResultToken = await _resource.GenerateTokenJwt(request.Usuario);
                if (!ResultToken.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }

                
                colaboradorDTO.Token = ResultToken.Data;
                //DateTime fechaAsignacion = DateTime.ParseExact(request.Fecha, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                if (colaboradorDTO.IdRol == 2 || colaboradorDTO.IdRol == 3 || colaboradorDTO.IdRol == 7)
                {
                    var ResultGasolinera = await _resourceEstacion.GetGasolinera();
                    if (ResultGasolinera.Success)
                    {
                        var gasolinera = ResultGasolinera.Data;
                        colaboradorDTO.FechaCorte = gasolinera.Fecha;
                        colaboradorDTO.Turno = gasolinera.TurnoActual;

                        var ResultAsignacion = await _resourceAsignacion.GetAsignacion(colaboradorDTO.NumeroVentuk, gasolinera.Fecha);
                        if (ResultAsignacion.Success)
                        {
                            List<AsignacionColaboradorTurno> lstAsignaciones = new List<AsignacionColaboradorTurno>();
                            AsignacionColaboradorTurno asignacion;
                            var turnos = ResultAsignacion.Data.Select(a => new { a.Turno }).Distinct().ToList();
                            foreach (var turno in turnos)
                            {
                                asignacion = new AsignacionColaboradorTurno();
                                asignacion.Turno = turno.Turno;
                                asignacion.Asignacion = ResultAsignacion.Data
                                    .Where(a => a.Turno == turno.Turno)
                                    .Select(x => new AsignacionDTO { IdBomba = x.IdBomba, Bomba = x.Bomba, noIsla = x.noIsla, NoBomba = x.NoBomba })
                                    .ToList();
                                lstAsignaciones.Add(asignacion);
                            }
                            colaboradorDTO.Asignacion = lstAsignaciones;

                        }
                        //else if (colaboradorDTO.IdRol == 2)
                        //{

                        //}
                        else
                        {
                            colaboradorDTO.Asignacion = new List<AsignacionColaboradorTurno>();
                            colaboradorDTO.Asignacion.Add(new AsignacionColaboradorTurno { Turno = ResultGasolinera.Data.TurnoActual });
                        }
                    }

                }
                else if (colaboradorDTO.IdRol == 15)
                {
                    var ResultGasolinera = await _resourceEstacion.GetGasolinera();
                    
                    if (!ResultGasolinera.Success)
                    {
                        Result.Success = false;
                        Result.Error = ResultGasolinera.Error;
                        Result.Message = ResultGasolinera.Message;
                        return Result;
                    }
                    
                    var gasolinera = ResultGasolinera.Data;
                    colaboradorDTO.FechaCorte = gasolinera.Fecha;
                    colaboradorDTO.Turno = gasolinera.TurnoActual;

                    colaboradorDTO.Asignacion = new List<AsignacionColaboradorTurno>();
                    colaboradorDTO.Asignacion.Add(new AsignacionColaboradorTurno { Turno = ResultGasolinera.Data.TurnoActual });
                    //if (colaboradorDTO.IdRol == 15)
                    //{
                        var ResultEstacion = await _resourceEstacion.GetEstacion();
                        if (!ResultEstacion.Success)
                        {
                            Result.Success = false;
                            Result.Error = ResultEstacion.Error;
                            Result.Message = ResultEstacion.Message;
                            return Result;
                        }
                        var Estacion = ResultEstacion.Data;
                        colaboradorDTO.NoEstacion = Estacion.NoEstacion;
                        colaboradorDTO.Estacion = Estacion.Estacion;
                    //}
                }

                colaboradorDTO.Impresoras = new List<Impresoras>();
                var ResultImpresoras = await _resourceImpresoras.GetImpresoras();
                if (ResultImpresoras.Success)
                {
                    colaboradorDTO.Impresoras = ResultImpresoras.Data.ToList();
                }

                colaboradorDTO.Configuracion = new ConfiguracionAppDTO();
                colaboradorDTO.Configuracion.Modulos = new List<ModuloCQRS.DTO.ModuloDTO>();

                var ResultLicencia = await _resourceEstacion.GetLicenciaGetnet();
                if (ResultLicencia.Success)
                {
                    colaboradorDTO.Configuracion.LicenciaTerminal = ResultLicencia.Data;
                }

                
                var ResultModulos = await _resourceModulo.GetModulos();
                if (ResultModulos.Success)
                {
                    var Modulos = ResultModulos.Data.ToList();
                    colaboradorDTO.Configuracion.Modulos = Modulos;
                }

                Result.Success = true;
                Result.Error = "";
                Result.Message = "Inicio de sesión exitoso";
                Result.Data = colaboradorDTO;

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
