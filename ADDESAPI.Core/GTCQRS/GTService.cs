using ADDESAPI.Core.GTCQRS.DTO;
using ADDESAPI.Core.PresetCQRS;
using ADDESAPI.Core.PresetCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ADDESAPI.Core.PreventaCQRS;
using ADDESAPI.Core.PreventaCQRS.DTO;
using ADDESAPI.Core.EstacionCQRS;

namespace ADDESAPI.Core.GTCQRS
{
    public class GTService : IGTService
    {
        private readonly IGTResource _resource;
        private readonly IPresetResource _resourcePreset;
        private readonly IPreventaResource _resourcePreventa;
        private readonly IEstacionResource _resourceEstacion;

        public GTService(IGTResource resource, IPresetResource resourcePreset, IPreventaResource resourcePreventa, IEstacionResource resourceEstacion)
        {
            _resource = resource;
            _resourcePreset = resourcePreset;
            _resourcePreventa = resourcePreventa;
            _resourceEstacion = resourceEstacion;
        }

        public async Task<Result> Preset(PresetDTO request)
        {
            Result Result = new Result();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = request.Bomba.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Grado", Value = request.Grado.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(double), ParameterName = "Cantidad", Value = request.Cantidad.ToString() });
                //valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "UMedida", Value = request.UMedida.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                //string command = "";
                //if (request.UMedida == 1)
                //{
                //    double cantidad = request.Cantidad * 100;
                //    command = $"preset|{request.Bomba}|{request.Grado}|{cantidad}|14|{request.UMedida}|D";
                //}
                //else
                //{
                //    command = $"preset|{request.Bomba}|{request.Grado}|{request.Cantidad}|14|{request.UMedida}";
                //}

                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;

                var ResultStatus = await GetStatusB(request.Bomba, token);
                if (!ResultStatus.Success)
                {
                    Result.Success = ResultStatus.Success;
                    Result.Error = ResultStatus.Error;
                    Result.Message = ResultStatus.Message;
                    return Result;
                }

                //var data = new PresetGTDTO { 
                //    Bomba = request.Bomba,
                //    Grado = request.Grado,
                //    CantOimp = request.Cantidad,
                //    CodDespachador = request.NoEmpleado,
                //    LAD = 1,
                //    tiptrn = request.TipoPago,
                //    ClientApp = new PresetClienteAppGTDTO { QrPago = request.QrPago, RFC = request.RFC, QrCupon = request.QrCupon, Cupon = request.Cupon, Total = request.Total, Descuento = request.Descuento, Estacion = request. }
                //};
                //string jsonPreset = JsonConvert.SerializeObject(data);

                
                var ResultPreset = await _resource.SetPreset(token, request);
                Result.Success = ResultPreset.Success;
                Result.Error = ResultPreset.Error;
                Result.Message = ResultPreset.Message;

                if (Result.Success)
                {
                    if (request.IdPreventa > 0)
                    {
                        var setPreventa = await _resourcePreventa.SetStatus(new SetPreventaStatusDTO { Id = request.IdPreventa, Estatus = 2 });
                    }
                    
                }

                //  var ResultPreset = await _resource.SendCommand(command, token);
                //    var savePresetR = await _resourcePreset.PresetGatewayRes(new PresetGatewayRes { Fecha = DateTime.Now, DespachoAnterior = 0, NoBomba = request.Bomba, Preset = command, RespuestaGateway = ResultPreset.Data.ToString() });
                //    if (ResultPreset.Success)
                //    {
                //        var data = ResultPreset.Data;
                //        if (data.executed)
                //        {
                //            var response = ResultPreset.Data.response.Split('|');
                //            string info = response[0];
                //            string estatus = response[1];

                //            if (estatus == "1")
                //            {
                //                var savePreset = await _resourcePreset.SavePreset(request);
                //                if (!savePreset.Success)
                //                {
                //                    Result.Success = false;
                //                    Result.Error = "";
                //                    Result.Message = $"Error al guardar el preset en SQL, el Preset se envio al gateway correctamente";
                //                }
                //                Result.Success = true;
                //                Result.Error = "";
                //                Result.Message = $"Preset enviado";

                //            }
                //            else
                //            {
                //                string mensaje = response[2];
                //                Result.Success = false;
                //                Result.Error = "Error en preset";
                //                Result.Message = $"{mensaje}. {response}";
                //            }
                //        }
                //        else
                //        {
                //            Result.Success = false;
                //            Result.Error = "Error en la ejecucion";
                //            Result.Message = data.response.ToString();
                //        }
                //    }
                //    else
                //    {
                //        Result.Success = false;
                //        Result.Error = ResultPreset.Error;
                //        Result.Message = ResultPreset.Message;
                //    }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error en Preset GT";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }

        public async Task<Result> GetStatusB(int bomba, string token)
        {
            Result Result = new Result();
            try
            {
                string statusCmd = $"getStatus|{bomba}|14";
                var ResultStatus = await _resource.SendCommand(statusCmd, token);
                if (!ResultStatus.Success)
                {
                    Result.Success = false;
                    Result.Message = ResultStatus.Message;
                    Result.Error = ResultStatus.Error;
                    return Result;
                }
                var dataStatus = ResultStatus.Data;
                var responseStatus = dataStatus.response.Split('|');
                string info = responseStatus[0];

                info = info.Replace("\0", "");
                if (info.ToLower().Contains("transacción inválida") || info.ToLower().Contains("transaccion invalida"))
                {
                    Result.Success = false;
                    Result.Message = info;
                    Result.Error = "Error al obtener el Estatus de la bomba";
                    return Result;
                }
                string status = responseStatus[1];

                switch (status)
                {
                    case "0":
                        Result.Success = false;
                        Result.Message = $"{responseStatus[2]}";
                        Result.Error = "Error";
                        break;
                    case "48":
                        Result.Success = false;
                        Result.Message = "Dispensario fuera de Línea";
                        Result.Error = "Error";
                        break;
                    case "49":
                        Result.Success = true;
                        Result.Message = "SGPM_S_IDLE(En Espera)";
                        Result.Error = "";
                        break;
                    case "50":
                        Result.Success = false;
                        Result.Message = "Bomba despachando";
                        Result.Error = "Error";
                        break;
                    case "51":
                        Result.Success = false;
                        Result.Message = "Registrando el Despacho";
                        Result.Error = "Error";
                        break;
                    case "53":
                        Result.Success = false;
                        Result.Message = "La manguera se encuentra descolgada. Pide Autorización";
                        Result.Error = "Error";
                        break;
                    case "54":
                        Result.Success = false;
                        Result.Message = "Bomba inhabilitada";
                        Result.Error = "Error";
                        break;
                    case "55":
                        Result.Success = false;
                        Result.Message = "Dispensario en ERROR";
                        Result.Error = "Error";
                        break;
                    case "56":
                        Result.Success = false;
                        Result.Message = "Bomba detenida";
                        Result.Error = "Error";
                        break;
                    case "57":
                        Result.Success = false;
                        Result.Message = "Hay un preset en espera (Para cancelar descuelgue y cuelgue la manguera)";
                        Result.Error = "Error";
                        break;
                    
                }
                return Result;
                
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error en Preset GT";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<Result> SetTypeBombas(SetBombasTypeDTO request)
        {
            Result Result = new Result();
            try
            {
                if (request.selfService == null && request.fullService == null)
                {
                    Result.Success = false;
                    Result.Error = "Error en datos de entrada";
                    Result.Message = "No se recibio correctamente el listado de bombas";
                    return Result;
                }
                else if (request.selfService == null)
                {
                    request.selfService = new List<int>();
                }
                else if (request.fullService == null)
                {
                    request.fullService = new List<int>();
                }

                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;

                var typeList = new BombasTypeDTO();
                typeList.fullService = request.fullService;
                typeList.selfService = request.selfService;

                if (typeList.fullService.Where(i => i < 1).Count() > 0 || typeList.selfService.Where(i => i < 1).Count() > 0)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = "Se detecto un valor no valido en la lista de bombas";
                    return Result;
                }

                string jsonString = System.Text.Json.JsonSerializer.Serialize(typeList);

                Result = await _resource.SetTypeBombas(token, jsonString);

                if (Result.Success)
                {
                    var ResultBitacora = await _resourceEstacion.SetDispensarioCambios(0, request.Usuario, 0, $"Se cambiaron todas las bombas desde APP Vendedor {jsonString}");
                }

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al cambiar dispensarios";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<BombasTypeDTO>> GetTypeBombas()
        {
            ResultSingle<BombasTypeDTO> Result = new ResultSingle<BombasTypeDTO>();
            try
            {
                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;

                var data = await _resource.GetTypeBombas(token);
                if (data.Success)
                {
                    Result.Success = data.Success;
                    Result.Error = data.Error;
                    Result.Message = data.Message;
                    Result.Data = new BombasTypeDTO();
                    Result.Data.fullService = new List<int>();
                    Result.Data.selfService = new List<int>();
                    foreach (var item in data.Data)
                    {
                        if (item.typeOfOperation == 0)
                        {
                            Result.Data.selfService.Add(item.bomba);
                        }
                        else
                        {
                            Result.Data.fullService.Add(item.bomba);
                        }
                    }
                }
                else
                {
                    Result.Success = data.Success;
                    Result.Error = data.Error;
                    Result.Message = data.Message;
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al obtener dispensarios";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<Result> SetTypeBomba(SetBombaTypeDTO request)
        {
            Result Result = new Result();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = request.Bomba.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Usuario", Value = request.Usuario });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Modo", Value = request.Modo.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;


                if (request.Bomba < 1)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = "El número de bombas no es valido";
                    return Result;
                }
                else if (request.Modo != 0 && request.Modo != 1)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"El modo {request.Modo} no es valido para el dispensario";
                    return Result;
                }

                Result = await _resource.SetTypeBomba(token, request.Bomba, request.Modo);

                if (Result.Success)
                {
                    var ResultBitacora = await _resourceEstacion.SetDispensarioCambios(request.Bomba, request.Usuario, request.Modo, "Se cambio desde APP Vendedor");
                    var ResultRestartSGPM = await _resource.RestartSGPM(token);
                    if (!ResultRestartSGPM.Success)
                    {
                        Result.Success = false;
                        Result.Error = $"{ResultRestartSGPM.Message}";
                        Result.Message = "El cambio se ralizo correctamente pero ocurrio un error al reiniciar el monitor de bombas";
                    }
                }

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al cambiar dispensarios";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<ResultMultiple<EstructuraBombaDTO>> GetEstructuraBomba(GetEstructuraDTO request)
        {
            ResultMultiple<EstructuraBombaDTO> Result = new ResultMultiple<EstructuraBombaDTO>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = request.Bomba.ToString() });

                var ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;

                Result = await _resource.GetEstructuraBomba(token, request.Bomba);
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al obtener la estructura";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<Result> CancelarPreset(GetEstructuraDTO request)
        {
            Result Result = new Result();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = request.Bomba.ToString() });

                var ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;

                Result = await _resource.CancelarPreset(token, request.Bomba);
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al cancelar el preset";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<ApiGtAnticipoRes>> AddAnticipo(AddAnticipoDTO req)
        {
            ResultSingle<ApiGtAnticipoRes> Result = new ResultSingle<ApiGtAnticipoRes>();
            try
            {

                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data;

                int codval = -269;
                if (req.IdTipoPago == 51)
                    codval = -267;
                else if (req.IdTipoPago == 52)
                    codval = -268;

                var data = new { fch = req.FechaCG, nrotur = req.Turno, codisl = req.IslaId, codres = req.NoEmpleado, fchcor = req.FchCor, codval = codval, can = req.Monto, mto = req.Monto };

                string jsonString = System.Text.Json.JsonSerializer.Serialize(data);

                Result = await _resource.AddAnticipo(token, jsonString);


            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al cambiar dispensarios";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
    }
}
