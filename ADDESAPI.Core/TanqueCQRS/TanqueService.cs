using ADDESAPI.Core.EstacionCQRS;
using ADDESAPI.Core.TanqueCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.TanqueCQRS
{
    public class TanqueService : ITanqueService
    {
        private readonly ITanqueResource _resource;
        private readonly IEstacionResource _resourceEstacion;

        public TanqueService(ITanqueResource resource, IEstacionResource resourceEstacion)
        {
            _resource = resource;
            _resourceEstacion = resourceEstacion;
        }

        public async Task<ResultSingle<LecturasDTO>> GetLecturas()
        {
            ResultSingle<LecturasDTO> Result = new ResultSingle<LecturasDTO>();
            LecturasDTO LecturasDTO = new LecturasDTO();

            try
            {
                var ResultEstacion = await _resourceEstacion.GetEstacion();
                if (!ResultEstacion.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultEstacion.Error;
                    Result.Message = ResultEstacion.Message;
                    return Result;
                }
                var Estacion = ResultEstacion.Data;
                LecturasDTO.Estacion = $"{Estacion.NoEstacion} {Estacion.Estacion}";
                LecturasDTO.RFC = Estacion.RFC;
                LecturasDTO.RazonSocial = Estacion.RazonSocial;
                LecturasDTO.Direccion = Estacion.Direccion;
                LecturasDTO.FechaImpresion = DateTime.Now;
                LecturasDTO.Lecturas = new List<LecturaDTO>();

                var ResultTanques = await _resource.GetTanques();
                if (!ResultTanques.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultTanques.Error;
                    Result.Message = ResultTanques.Message;
                    return Result;
                }
                var Tanques = ResultTanques.Data;

                foreach (var t in Tanques)
                {
                    var ResultLectura = await _resource.GetUltimaLectura(t.NumeroTanque);
                    if (ResultLectura.Success)
                    {
                        var lectura = ResultLectura.Data;
                        lectura.Porcentaje = (lectura.Volumen * 100) / t.CapacidadOperacional;
                        lectura.PorLlenar = (t.CapacidadOperacional * .95) - lectura.Volumen;
                        lectura.CodProducto = t.CodigoProducto;
                        lectura.Producto = t.Producto;
                        LecturasDTO.Lecturas.Add(lectura);
                    }
                }

                if (LecturasDTO.Lecturas.Count > 0)
                {
                    
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "";
                    Result.Data = LecturasDTO;
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "Error al obtener lecturas";
                    Result.Message = "No se encontraron lecturas";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al obtener lecturas";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
    }
}
