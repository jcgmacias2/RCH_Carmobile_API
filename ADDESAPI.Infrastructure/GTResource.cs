using ADDESAPI.Core;
using ADDESAPI.Core.GTCQRS;
using ADDESAPI.Core.GTCQRS.DTO;
using ADDESAPI.Core.PresetCQRS.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Infrastructure
{
    public class GTResource : IGTResource
    {
        private readonly IConfiguration _configuration;
        private readonly string _url;
        private readonly string _userGT;
        private readonly string _pwdGT;
        private readonly string _apiToken;
        private readonly string _apiCommand;
        private readonly string _apiFamilias;
        private readonly string _apiPreset;
        private readonly int _estacion;

        public GTResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
            _url = _configuration["GT:URL"];
            _userGT = _configuration["GT:UsrGT"];
            _pwdGT = _configuration["GT:PwdGT"];
            _apiToken = _configuration["GT:ApiToken"];
            _apiCommand = _configuration["GT:ApiCommand"];
            _apiFamilias = _configuration["GT:ApiFamilias"];
            _apiPreset = _configuration["GT:ApiPreset"];
        }
        public async Task<ResultSingle<string>> GetToken()
        {
            ResultSingle<string> Result = new ResultSingle<string>();
            try
            {
                var request = new RestRequest(_apiToken, Method.Get);

                var options = new RestClientOptions(_url)
                {
                    Authenticator = new HttpBasicAuthenticator(_userGT, _pwdGT)
                };
                var client = new RestClient(options);

                request.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<GTLoginDTO>(response.Content);

                    if (r != null)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"Token generado";
                        Result.Data = r.Token;
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = $"Error";
                        Result.Message = $"Error al generar el Token de GT";
                    }
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"Error al generar el Token de GT. {_url} EstatusCode: {response.StatusCode }{response.StatusDescription}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error en Token GT";
                Result.Message = $"Error {ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<GTCommandResponse>> SendCommand(string command, string token)
        {
            ResultSingle<GTCommandResponse> Result = new ResultSingle<GTCommandResponse>();
            string _api = "";
            try
            {
                _api = $"{_apiCommand}?commandToSend={command}";
                var request = new RestRequest(_api, Method.Get);
                var authenticator = new JwtAuthenticator(token);
                var options = new RestClientOptions(_url)
                {
                    Authenticator = authenticator
                };
                var client = new RestClient(options);

                request.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<GTCommandResponse>(response.Content);

                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = $"Comando enviado";
                    Result.Data = r;

                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"Error al enviar el comando {_api}. {_url} EstatusCode: {response.StatusCode } {response.StatusDescription}";
                }

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al enviar el comando {_api} Excepcion: {ex.Message}";
            }
            return Result;
        }
        public async Task<Result> SetPreset(string token, PresetDTO preset/*, string jsonPreset*/)
        {
            Result Result = new Result();

            try
            {
                var data = new PresetGTDTO
                {
                    Bomba = preset.Bomba,
                    Grado = preset.Grado,
                    CantOimp = preset.Cantidad,
                    CodDespachador = preset.NoEmpleado,
                    LAD = 1,
                    tiptrn = preset.TipoPago,
                    ClientApp_RLS = new PresetClienteAppGTDTO { QrPago = preset.QrPago, RFC = preset.RFC, QrCupon = preset.QrCupon, Cupon = preset.Cupon, Total = preset.Total, Descuento = preset.Descuento, Estacion = _estacion}
                };
                string jsonPreset = JsonConvert.SerializeObject(data);

                var request = new RestRequest(_apiPreset, Method.Post);
                var authenticator = new JwtAuthenticator(token);
                var options = new RestClientOptions(_url)
                {
                    Authenticator = authenticator
                };
                var client = new RestClient(options);
                request.AddJsonBody(jsonPreset);
                request.RequestFormat = DataFormat.Json;
                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<GTPresetResponse>(response.Content);

                    if (r.respuesta == 1)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"";
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"Respuesta: {r.respuesta}, Mensaje: {r.mensaje}";
                    }
                    
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"{response.Content}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al enciar el preset {_apiPreset} Excepcion: {ex.Message}";
            }
            return Result;
        }
    }
}
