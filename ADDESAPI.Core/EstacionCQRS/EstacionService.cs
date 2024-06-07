using ADDESAPI.Core.EstacionCQRS.DTO;
using ADDESAPI.Core.GTCQRS;
using ADDESAPI.Core.GTCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.EstacionCQRS
{
    public class EstacionService : IEstacionService
    {
        private readonly IEstacionResource _resource;
        private readonly IGTResource _resourceGT;

        public EstacionService(IEstacionResource resource, IGTResource resourceGT)
        {
            _resource = resource;
            _resourceGT = resourceGT;
        }
        public async Task<ResultMultiple<vBombas>> GetBombas()
        {
            return await _resource.GetBombas();
        }
        public async Task<ResultMultiple<EstacionCombustiblesDTO>> GetCombustibles()
        {
            ResultMultiple <EstacionCombustiblesDTO> Result = new ResultMultiple<EstacionCombustiblesDTO>();
            List<EstacionCombustiblesDTO> Combustibles = new List<EstacionCombustiblesDTO>();
            try
            {
                var ResultTanques = await _resource.GetTanques();
                if (!ResultTanques.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultTanques.Error;
                    Result.Message = ResultTanques.Message;
                    return Result;
                }
                var tanques = ResultTanques.Data;
                foreach (var tanque in tanques)
                {
                    Combustibles.Add(new EstacionCombustiblesDTO { CodigoProducto = tanque.CodigoProducto, Producto = tanque.Producto, GradoProducto = tanque.GradoProducto });
                }

                Result.Success = true;
                Result.Error = "";
                Result.Message = "";
                Result.Data = Combustibles;
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultMultiple<PrecioBombaGtDTO>> GetPrecios(PrecioGtReqDTO req)
        {
            ResultMultiple<PrecioBombaGtDTO> Result = new ResultMultiple<PrecioBombaGtDTO>();
            List<PrecioBombaGtDTO> Precios = new List<PrecioBombaGtDTO>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = req.Bomba.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultCombustibles = await GetCombustibles();
                if (!ResultCombustibles.Success)
                {
                    Result.Success = ResultCombustibles.Success;
                    Result.Error = ResultCombustibles.Error;
                    Result.Message = ResultCombustibles.Message;
                    return Result;
                }
                var combustibles = ResultCombustibles.Data;

                var ResultToken = await _resourceGT.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;
                string command = "";

                foreach (var item in combustibles)
                {
                    command = $"getPrice|{req.Bomba}|{item.GradoProducto}|14";
                    var ResultPrecio = await _resourceGT.SendCommand(command, token);
                    
                    if (ResultPrecio.Success)
                    {
                        var data = ResultPrecio.Data;

                        if (data.executed)
                        {
                            var response = data.response.Split('|');
                            if (response.Length == 5)
                            {
                                string info = response[0];
                                string estatus = response[1];
                                if (estatus == "1")
                                {
                                    double valor = double.Parse(response[4]);

                                    if (valor > 0)
                                    {
                                        double precio = valor / 1000;
                                        Precios.Add(new PrecioBombaGtDTO
                                        {
                                            Executed = true,
                                            Response = $"{data.response}",
                                            Bomba = req.Bomba,
                                            CodigoProducto = item.CodigoProducto,
                                            Grado = item.GradoProducto,
                                            Descipcion = item.Producto,
                                            Precio = precio
                                        });
                                    }
                                    else
                                    {
                                        Precios.Add(new PrecioBombaGtDTO
                                        {
                                            Executed = false,
                                            Response = $"No se encontro el precio. Response GT {data.response}",
                                            Bomba = req.Bomba,
                                            CodigoProducto = item.CodigoProducto,
                                            Grado = item.GradoProducto,
                                            Descipcion = item.Producto,
                                            Precio = 0
                                        });
                                    }
                                }
                                else
                                {

                                    string mensaje = response[2];
                                    Precios.Add(new PrecioBombaGtDTO
                                    {
                                        Executed = false,
                                        Response = $"{mensaje}. {data.response}",
                                        Bomba = req.Bomba,
                                        CodigoProducto = item.CodigoProducto,
                                        Grado = item.GradoProducto,
                                        Descipcion = item.Producto,
                                        Precio = 0
                                    });
                                }
                            }
                            else
                            {
                                Result.Success = false;
                                Result.Message = data.response;
                                Result.Error = "Error al obtener el precio";
                            }

                        }
                        else
                        {
                            Precios.Add(new PrecioBombaGtDTO { 
                                Executed = false, 
                                Response = data.response.ToString(),
                                Bomba = req.Bomba,
                                CodigoProducto = item.CodigoProducto,
                                Grado = item.GradoProducto,
                                Descipcion = item.Producto,
                                Precio = 0
                            });
                        }
                    }
                }
                Result.Success = true;
                Result.Error = "";
                Result.Message = "";
                Result.Data = Precios;
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error en Precio GT";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<vGasolinera>> GetGasolinera()
        {
            return await _resource.GetGasolinera();
        }
        public async Task<ResultSingle<TurnoActualDTO>> GetTurno()
        {
            ResultSingle<TurnoActualDTO> Result = new ResultSingle<TurnoActualDTO>();

            try
            {
                var ResultGasolinera = await _resource.GetGasolinera();
                if (!ResultGasolinera.Success)
                {
                    Result.Success = false;
                    Result.Error = ResultGasolinera.Error;
                    Result.Message = ResultGasolinera.Message;
                    return Result;
                }


                Result.Success = true;
                Result.Error = "";
                Result.Message = "";
                Result.Data = new TurnoActualDTO { Turno = ResultGasolinera.Data.TurnoActual };
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultMultiple<FormaPago>> GetFormasPago()
        {
            ResultMultiple<FormaPago> Result = new ResultMultiple<FormaPago>();

            try
            {
                Result = await _resource.GetFormasPago();

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
