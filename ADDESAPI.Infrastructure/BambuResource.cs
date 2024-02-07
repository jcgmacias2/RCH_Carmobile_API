using ADDESAPI.Core;
using ADDESAPI.Core.BambuCQRS;
using ADDESAPI.Core.BambuCQRS.DTO;
using Microsoft.Extensions.Configuration;
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
    public class BambuResource : IBambuResource
    {
        private readonly IConfiguration _configuration;
        private readonly string _url;
        private readonly string _emailBambu;
        private readonly string _passwordBambu;
        private readonly string _apiToken;
        private readonly string _apiFuelingQr;
        private readonly string _apiFueling;

        public BambuResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration["Bambu:URL"];
            _emailBambu = _configuration["Bambu:EmailBambu"];
            _passwordBambu = _configuration["Bambu:PasswordBambu"];
            _apiToken = _configuration["Bambu:ApiToken"];
            _apiFuelingQr = _configuration["Bambu:fuelingQr"];
            _apiFueling = _configuration["Bambu:fueling"];
        }

        public async Task<ResultSingle<BambuTokenDTO>> GetToken()
        {
            ResultSingle<BambuTokenDTO> Result = new ResultSingle<BambuTokenDTO>();
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                //var client = new RestClient(_url);
                var requestB = new RestRequest(_apiToken, Method.Post);
                var options = new RestClientOptions(_url)
                {
                    RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
                };
                //requestB.AddHeader("Access-Control-Allow-Origin", "*");
                var client = new RestClient(options);
                var Data = new
                {
                    email = _emailBambu,
                    password = _passwordBambu
                };
                requestB.AddJsonBody(Data);
                requestB.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(requestB);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultBambu<BambuTokenDTO>>(response.Content);
                    var TokenBambu = r.data;

                    if (TokenBambu.valid)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"Token generado";
                        Result.Data = TokenBambu;
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = $"Error al generar el Token de Bambu. {TokenBambu.error}";
                        Result.Message = TokenBambu.message;
                    }
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"Error al generar el Token de Bambu. {_url}{_apiToken} EstatusCode: {response.StatusCode } Desc{response.StatusDescription} Response {response.Content}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error {ex.Message}";
            }
            return Result;
        }
        public async Task<Result> FuelingQR(RequestFuelingQrDTO request, string token)
        {
            Result Result = new Result();
            try
            {
                
                var requestB = new RestRequest(_apiFuelingQr, Method.Post);                
                var authenticator = new JwtAuthenticator(token);
                var options = new RestClientOptions(_url)
                {
                    Authenticator = authenticator
                };
                var client = new RestClient(options);

                var Data = new
                {
                    user_id = request.user_id,
                    ticket = request.ticket,
                    charge = request.charge,
                    fuel_type = request.fuel_type,
                    date = request.date
                };

                requestB.AddJsonBody(Data);
                requestB.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(requestB);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultBambu<string>>(response.Content);
                    var TicketBambu = r.data;

                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = $"{TicketBambu}";

                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"Error al enviar el Ticket a Bambu. {_url} EstatusCode: {response.StatusCode } {response.StatusDescription}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error {ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<QR>> GetQrPago(RequestGetQrDTO request, string token)
        {
            ResultSingle<QR> Result = new ResultSingle<QR>();
            try
            {
                string _api = $"{_apiFueling}/{request.QR}";
                var requestB = new RestRequest(_api, Method.Get);
                var authenticator = new JwtAuthenticator(token);
                var options = new RestClientOptions(_url)
                {
                    Authenticator = authenticator
                };
                var client = new RestClient(options);


                requestB.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(requestB);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultBambu<BambuFuelingDTO>>(response.Content);
                    if (r.data.valid)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"QR Encontrado";
                        var data = r.data.fueling;
                        Result.Data = new QR {
                            EstacionPago = data.branch_Office.id_branch,
                            Combustible = data.fuel_type.value,
                            FormaPago = data.payment_method,
                            ReferenciaPago = data.payment_reference,
                            Precio = data.price_per_liter,
                            Litros = data.liters,
                            Total = data.total,
                            Descuento = data.discount,
                            TotalConDescuento = data.full_discount,
                            FechaCupon = data.succeded_at.fulldate
                        };
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = $"{r.data.error}";
                        Result.Message = $"{r.data.internal_message}. {r.data.message}";
                    }                    
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"Error al obtener el Cupon. {_url}/{_api} EstatusCode: {response.StatusCode } {response.StatusDescription}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error {ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<QR>> SetQrPago(RequestSetQrDTO request, string token)
        {
            ResultSingle<QR> Result = new ResultSingle<QR>();
            try
            {
                string _api = $"{_apiFueling}/{request.QR}";
                var requestB = new RestRequest(_api, Method.Post);
                var authenticator = new JwtAuthenticator(token);
                var options = new RestClientOptions(_url)
                {
                    Authenticator = authenticator
                };
                var client = new RestClient(options);
                var Data = new
                {
                    id_branch = request.Estacion,
                    id_pump = request.Bomba,
                    id_operator = request.NoEmpleado
                };

                requestB.AddJsonBody(Data);

                requestB.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(requestB);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultBambu<BambuFuelingDTO>>(response.Content);
                    if (r.data.valid)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"QR Encontrado";
                        var data = r.data.fueling;
                        Result.Data = new QR
                        {
                            EstacionPago = data.branch_Office.id_branch,
                            Combustible = data.fuel_type.value,
                            FormaPago = data.payment_method,
                            ReferenciaPago = data.payment_reference,
                            Precio = data.price_per_liter,
                            Litros = data.liters,
                            Total = data.total,
                            Descuento = data.discount,
                            TotalConDescuento = data.full_discount,
                            FechaCupon = data.succeded_at.fulldate
                        };
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = $"{r.data.error}";
                        Result.Message = $"{r.data.internal_message}. {r.data.message}";
                    }
                }
                else
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultBambu<BambuFuelingDTO>>(response.Content);
                    Result.Success = false;
                    Result.Error = "Error al utilizar QR";
                    Result.Message = $"{r.data.message}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al utilizar QR";
                Result.Message = $"Error {ex.Message}";
            }
            return Result;
        }
        public async Task<Result> CancelQrPago(RequestGetQrDTO request, string token)
        {
            Result Result = new Result();
            try
            {
                string _api = $"{_apiFueling}/{request.QR}";
                var requestB = new RestRequest(_api, Method.Patch);
                var authenticator = new JwtAuthenticator(token);
                var options = new RestClientOptions(_url)
                {
                    Authenticator = authenticator
                };
                var client = new RestClient(options);

                requestB.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(requestB);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultBambu<CancelQr>>(response.Content);
                    if (r.data.valid)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"{r.data.message}";
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = $"Error al cancelar el QR";
                        Result.Message = $"{r.data.message}. {r.data.message}";
                    }
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error al cancelar QR";
                    Result.Message = $"Error al obtener el Cupon. {_url}/{_api} EstatusCode: {response.StatusCode } {response.StatusDescription}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al cancelar QR";
                Result.Message = $"Error {ex.Message}";
            }
            return Result;
        }
    }
}
