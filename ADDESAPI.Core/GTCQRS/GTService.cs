using ADDESAPI.Core.GTCQRS.DTO;
using ADDESAPI.Core.PresetCQRS;
using ADDESAPI.Core.PresetCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ADDESAPI.Core.GTCQRS
{
    public class GTService : IGTService
    {
        private readonly IGTResource _resource;
        private readonly IPresetResource _resourcePreset;

        public GTService(IGTResource resource, IPresetResource resourcePreset)
        {
            _resource = resource;
            _resourcePreset = resourcePreset;
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
       
    }
}
