using ADDESAPI.Core;
using ADDESAPI.Core.GTCQRS;
using ADDESAPI.Core.GTCQRS.DTO;
using ADDESAPI.Core.PresetCQRS.DTO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RepoDb;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private readonly string _apiSetTypeList;
        private readonly string _apiSetType;
        private readonly string _apiBombasGetStatusAll;
        private readonly string _apiRestartSGPM;
        private readonly string _apiEstructuraBomba;
        private readonly string _apiCancelarPreset;
        private readonly string _apiAnticipo;
        private readonly int _estacion;
        private readonly int _gasolinera;
        public readonly string _connectionString;
        public readonly string _connectionStringCG;

        public GTResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
            _url = _configuration["GT:URL"];
            _userGT = _configuration["GT:UsrGT"];
            _pwdGT = _configuration["GT:PwdGT"];
            _apiToken = _configuration["GT:ApiToken"];
            _apiCommand = _configuration["GT:ApiCommand"];
            _apiFamilias = _configuration["GT:ApiFamilias"];
            _apiPreset = _configuration["GT:ApiPreset"];
            _apiSetTypeList = _configuration["GT:ApiBombasTypeList"];
            _apiSetType = _configuration["GT:ApiBombaSetType"];
            _apiBombasGetStatusAll = _configuration["GT:ApiBombasGetStatusAll"];
            _apiRestartSGPM = _configuration["GT:ApiRestartSGPM"]; 
            _apiEstructuraBomba = _configuration["GT:ApiEstructuraBomba"];
            _apiCancelarPreset = _configuration["GT:ApiCancelarPreset"];
            _apiAnticipo = _configuration["GT:ApiAnticipo"];
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _connectionStringCG = _configuration["ConnectionStrings:CGConnection"];
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
        public async Task<Result> SetPreset(string token, PresetDTO preset)
        {
            Result Result = new Result();

            try
            {
                //PresetClienteAppGTDTO clientApp = new PresetClienteAppGTDTO{ QrPago = preset.QrPago, RFC = preset.RFC, QrCupon = preset.QrCupon, Cupon = preset.Cupon, Total = preset.Total, Descuento = preset.Descuento, Estacion = _estacion };
                if (preset.Moneda == null || preset.Moneda == "")
                {
                    preset.Moneda = "MXN";
                }

                PresetClienteAppGTDTO clientApp = new PresetClienteAppGTDTO();
                if (preset.TipoPago == 0 && preset.Moneda == "MXN" && preset.Descuento == 0 && preset.CardNumber == "")
                {
                    clientApp.Tipo = 0;
                    clientApp.jarreoApp = new PresetJarreoAppGTDTO();
                    clientApp.USD = new PresetDolaresAppGTDTO();
                    clientApp.acumular = new PresetAcumularAppGTDTO();
                    clientApp.redimir = new PresetRedimirAppGTDTO();
                }
                else if ((preset.TipoPago == 0 || preset.TipoPago == 51 || preset.TipoPago == 52) && preset.Moneda == "MXN" && preset.Descuento > 0 && preset.CardNumber != "")
                {
                    clientApp.Tipo = 1;
                    clientApp.jarreoApp = new PresetJarreoAppGTDTO();
                    clientApp.USD = new PresetDolaresAppGTDTO();
                    clientApp.acumular = new PresetAcumularAppGTDTO();
                    clientApp.redimir = new PresetRedimirAppGTDTO { Estacion = _estacion, Gasolinera = _gasolinera, Transaccion = 0, Bomba = preset.Bomba, CardNumber = preset.CardNumber, LitrosRedimir = preset.LitrosRedimir, Descuento = preset.Descuento, Precio = 0, Cantidad = 0, Total = 0, NoEmpleado = preset.NoEmpleado, Vendedor = preset.Nombre, ProgramId = preset.ProgramId, BrandId = preset.BrandId, Usuario = "Preset", Cliente = preset.Cliente, PromoDesc = preset.PromoDesc, Empresa = preset.Empresa };
                }
                else if (preset.TipoPago == 65)
                {
                    clientApp.Tipo = 2;
                    clientApp.jarreoApp = new PresetJarreoAppGTDTO { NoEstacion = _estacion, Gasolinera = _gasolinera, Transaccion = 0, Tipo = "Autojarreo", Bomba = preset.Bomba, CodProducto = 0, Producto = "", Cantidad = 0, Importe = 0, FechaDespacho = "", Usuario = preset.NoEmpleado.ToString() };
                    clientApp.USD = new PresetDolaresAppGTDTO();
                    clientApp.acumular = new PresetAcumularAppGTDTO();
                    clientApp.redimir = new PresetRedimirAppGTDTO();
                }
                else if (preset.TipoPago == 74)
                {
                    clientApp.Tipo = 2;
                    clientApp.jarreoApp = new PresetJarreoAppGTDTO { NoEstacion = _estacion, Gasolinera = _gasolinera, Transaccion = 0, Tipo = "Jarreo", Bomba = preset.Bomba, CodProducto = 0, Producto = "", Cantidad = 0, Importe = 0, FechaDespacho = "", Usuario = preset.NoEmpleado.ToString() };
                    clientApp.USD = new PresetDolaresAppGTDTO();
                    clientApp.acumular = new PresetAcumularAppGTDTO();
                    clientApp.redimir = new PresetRedimirAppGTDTO();
                }
                else if (preset.TipoPago == 0 && preset.Moneda == "USD" && preset.Descuento == 0)
                {
                    clientApp.Tipo = 3;
                    clientApp.jarreoApp = new PresetJarreoAppGTDTO();
                    clientApp.USD = new PresetDolaresAppGTDTO { Estacion = _estacion, Gasolinera = _gasolinera, Transaccion = 0, NoEmpleado = preset.NoEmpleado, Nombre = preset.Nombre, Turno = 0, Fecha = "", Hora = "", FechaCorte = "", Bomba = preset.Bomba, ImporteDespacho = 0, DolaresRecibidos = preset.UsdRecibidos, TipoCambio = preset.TipoCambio, CambioUSD = preset.CambioUSD, CambioMXN = preset.CambioMXN, FechaCG = 0, CardNumber = preset.CardNumber, Cliente = preset.Cliente, ProgramId = preset.ProgramId, BrandId = preset.BrandId, Empresa = preset.Empresa, LitrosRedimir = preset.LitrosRedimir, Descuento = preset.Descuento};
                    clientApp.USD.Detalle = new List<PresetDolaresDetAppGTDTO>();
                    clientApp.USD.Detalle.Add(new PresetDolaresDetAppGTDTO { Estacion = _estacion, Gasolinera = _gasolinera, Transaccion = 0, Despacho = 0, Producto = 0, Cantidad = 0, Descripcion = "", Precio = 0, Subtotal = 0, IVA = 0, IEPS = 0, Total = 0, CuotaIEPS = 0, TasaIVA = 0 });
                    clientApp.acumular = new PresetAcumularAppGTDTO();
                    clientApp.redimir = new PresetRedimirAppGTDTO();
                }
                else if ((preset.TipoPago == 0 || preset.TipoPago == 51 || preset.TipoPago == 52) && preset.Moneda == "MXN" && preset.Descuento == 0 && preset.CardNumber != "")
                {
                    clientApp.Tipo = 4;
                    clientApp.jarreoApp = new PresetJarreoAppGTDTO();
                    clientApp.USD = new PresetDolaresAppGTDTO();
                    clientApp.acumular = new PresetAcumularAppGTDTO { Transaccion = 0, Estacion = _estacion, Gasolinera = _gasolinera, Fecha = "", Bomba = preset.Bomba, Total = 0, IdTipoPago = preset.TipoPago, CardNumber = preset.CardNumber, NoEmpleado = preset.NoEmpleado, Vendedor = preset.Nombre, ProgramId = preset.ProgramId, BrandId = preset.BrandId, Usuario = "Preset", Empresa = preset.Empresa };
                    clientApp.redimir = new PresetRedimirAppGTDTO();
                }
                else if (preset.TipoPago == 0 && preset.Moneda == "USD" && preset.Descuento > 0)
                {
                    clientApp.Tipo = 3;
                    clientApp.jarreoApp = new PresetJarreoAppGTDTO();
                    clientApp.USD = new PresetDolaresAppGTDTO { Estacion = _estacion, Gasolinera = _gasolinera, Transaccion = 0, NoEmpleado = preset.NoEmpleado, Nombre = preset.Nombre, Turno = 0, Fecha = "", Hora = "", FechaCorte = "", Bomba = preset.Bomba, ImporteDespacho = 0, DolaresRecibidos = preset.UsdRecibidos, TipoCambio = preset.TipoCambio, CambioUSD = preset.CambioUSD, CambioMXN = preset.CambioMXN, FechaCG = 0, CardNumber = preset.CardNumber, Cliente = preset.Cliente, ProgramId = preset.ProgramId, BrandId = preset.BrandId, Empresa = preset.Empresa, LitrosRedimir = preset.LitrosRedimir, Descuento = preset.Descuento, PromoDesc = "Combustible" };
                    clientApp.USD.Detalle = new List<PresetDolaresDetAppGTDTO>();
                    clientApp.USD.Detalle.Add(new PresetDolaresDetAppGTDTO { Estacion = _estacion, Gasolinera = _gasolinera, Transaccion = 0, Despacho = 0, Producto = 0, Cantidad = 0, Descripcion = "", Precio = 0, Subtotal = 0, IVA = 0, IEPS = 0, Total = 0, CuotaIEPS = 0, TasaIVA = 0 });
                    clientApp.acumular = new PresetAcumularAppGTDTO();
                    clientApp.redimir = new PresetRedimirAppGTDTO();
                }


                var data = new PresetGTDTO
                {
                    Bomba = preset.Bomba,
                    Grado = preset.Grado,
                    CantOimp = preset.Cantidad,
                    CodDespachador = preset.NoEmpleado,
                    LAD = 1,
                    tiptrn = preset.TipoPago,
                    ClientApp_RLS = clientApp
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
        public async Task<Result> SetTypeBombas(string token, string json)
        {
            Result Result = new Result();

            try
            {

                var request = new RestRequest($"{_apiSetTypeList}?restart=True" , Method.Post);
                var authenticator = new JwtAuthenticator(token);
                var options = new RestClientOptions(_url)
                {
                    Authenticator = authenticator
                };
                var client = new RestClient(options);
                request.AddJsonBody(json);
                request.RequestFormat = DataFormat.Json;
                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtSetTypeResponse>(response.Content);

                    if (r.edit && r.restart)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"El cambio se realizo con exito";
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"Respuesta Edit {r.edit} Restart{r.restart}";
                    }

                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtErrorResponse>(response.Content);
                    Result.Message = $"{data}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al enviar el preset {_apiPreset} Excepcion: {ex.Message}";
            }
            return Result;
        }
        public async Task<Result> SetTypeBomba(string token, int bomba, int modo)
        {
            Result Result = new Result();

            try
            {
                string api = $"{_apiSetType}/bomba/{bomba}/settype/{modo}";
                var request = new RestRequest(api, Method.Get);
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
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtSetTypeResponse>(response.Content);

                    if (r.edit)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"El cambio se realizo con exito";
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"Respuesta Edit {r.edit} Restart{r.restart}";
                    }

                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtErrorResponse>(response.Content);
                    Result.Message = $"{data}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al enviar el preset {_apiPreset} Excepcion: {ex.Message}";
            }
            return Result;
        }
        public async Task<ResultMultiple<ApiGtBombasResponse>> GetTypeBombas(string token)
        {
            ResultMultiple<ApiGtBombasResponse> Result = new ResultMultiple<ApiGtBombasResponse>();

            try
            {

                var request = new RestRequest($"{_apiBombasGetStatusAll}?withLastTask=false", Method.Get);
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
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ApiGtBombasResponse>>(response.Content);

                    if (r.Count > 0)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"El proceso se realizo con exito";
                        Result.Data = r;
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"No se encontraron registros Respuesta GT {response.Content}";
                    }

                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtErrorResponse>(response.Content);
                    Result.Message = $"{data.error}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al obtener las bombas {ex.Message}";
            }
            return Result;
        }
        public async Task<Result> RestartSGPM(string token)
        {
            Result Result = new Result();

            try
            {
                var request = new RestRequest(_apiRestartSGPM, Method.Get);
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
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtRestartSGPMResponse>(response.Content);

                    if (r.respuesta)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"El reinicio se realizo con exito";
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"Error al reiniciar el SGPM";
                    }

                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error al reiniciar el SGPM";
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtErrorResponse>(response.Content);
                    Result.Message = $"{data}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al reiniciar el SGPM";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<ResultMultiple<EstructuraBombaDTO>> GetEstructuraBomba(string token, int bomba)
        {
            ResultMultiple<EstructuraBombaDTO> Result = new ResultMultiple<EstructuraBombaDTO>();
            string _api = "";
            try
            {
                _api = $"{_apiEstructuraBomba}/{bomba}";
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
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EstructuraBombaDTO>>(response.Content);

                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = $"La estructura se consulto correctamente";
                    Result.Data = r;

                }
                else
                {
                    Result.Success = false;
                    Result.Error = $"Error al obtener la estructura de la boma {bomba}";
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtErrorResponse>(response.Content);
                    Result.Message = data.error;
                }

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = $"Error al obtener la estructura de la boma {bomba}";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<Result> CancelarPreset(string token, int bomba)
        {
            Result Result = new Result();

            try
            {
                string api = $"{_apiCancelarPreset}/{bomba}";
                var request = new RestRequest(api, Method.Get);
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
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtCancelPresetResponse>(response.Content);

                    if (r.respuesta == "OK")
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"El preset se cancelo correctamente";
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"Error al cancelar el preset";
                    }

                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error al cancelar el preset";
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtErrorResponse>(response.Content);
                    Result.Message = $"{data.error}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al reiniciar el SGPM";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<ApiGtAnticipoRes>> AddAnticipo(string token, string json)
        {
            ResultSingle<ApiGtAnticipoRes> Result = new ResultSingle<ApiGtAnticipoRes>();

            try
            {
                var request = new RestRequest(_apiAnticipo, Method.Post);
                var authenticator = new JwtAuthenticator(token);
                var options = new RestClientOptions(_url)
                {
                    Authenticator = authenticator
                };
                var client = new RestClient(options);
                request.AddJsonBody(json);
                request.RequestFormat = DataFormat.Json;
                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtAnticipoRes>(response.Content);

                    if (r != null)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"El anticipo se registro con exito";
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"{response.Content}";
                    }

                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiGtErrorResponse>(response.Content);
                    Result.Message = $"{data}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al registrar el anticipo";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        //public async Task<Result> SetDiscount(int transaccion, double discount, string cardNumber, string promotion, int idPromotion, double littersApp, string clientName)
        //{
        //    Result Result = new Result();
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionStringCG))
        //        {
        //            promotion = promotion.Replace("'", "");
        //            string sql = $"UPDATE HTI_Tasks SET Promotion = '{promotion}', ClientApp = '{cardNumber}', preset_discountPerLiter = {discount}, idPromotion = {idPromotion}, littersApp = {littersApp}, TaxData = '{clientName}' WHERE nrotrn = {transaccion}";
        //            var affectedRecords = connection.ExecuteNonQuery(sql);
        //            if (affectedRecords == 0)
        //            {
        //                Result.Success = false;
        //                Result.Error = "";
        //                Result.Message = "No se actualizo el despacho";
        //            }
        //            else
        //            {
        //                Result.Success = true;
        //                Result.Error = "";
        //                Result.Message = "Despacho actualizado";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Success = false;
        //        Result.Error = "";
        //        Result.Message = ex.Message;
        //    }
        //    return Result;
        //}
        //public async Task<ResultSingle<DiscountDTO>> GetDiscount(string transaccion)
        //{
        //    ResultSingle<DiscountDTO> Result = new ResultSingle<DiscountDTO>();
        //    try
        //    {
        //        string sql = $"SELECT idPromotion, ISNULL(Promotion, '') 'Promotion', ISNULL(ClientApp, '') 'ClientApp', preset_discountPerLiter, pointsApp, littersApp, ISNULL(TaxData, '') 'TaxData' FROM HTI_Tasks WHERE nrotrn ={transaccion}";

        //        using var connection = new SqlConnection(_connectionStringCG);
        //        var req = await connection.ExecuteQueryAsync<DiscountDTO>(sql);

        //        if (req == null || req.Count() == 0)
        //        {
        //            Result.Success = false;
        //            Result.Error = "";
        //            Result.Message = "No se encontraron registros";
        //        }
        //        else
        //        {
        //            Result.Success = true;
        //            Result.Error = "";
        //            Result.Message = "Registro encontrado";
        //            Result.Data = req.FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Success = false;
        //        Result.Error = "";
        //        Result.Message = ex.Message;
        //    }
        //    return Result;
        //}
        public async Task<Result> SetDescuento(int transaccion, double descuento, string cardNumber, string PromoDesc, int promoCode, double litrosRedimidos, string nombreCliente, int noEmpleado, string vendedor)
        {
            Result Result = new Result();
            try
            {
                using (var connection = new SqlConnection(_connectionStringCG))
                {
                    PromoDesc = PromoDesc.Replace("'", "");
                    string sql = @"INSERT INTO HTI_Descuentos(Estacion,  Gasolinera,  Transaccion,  Descuento,  CardNumber,  NombreCliente,  litrosRedimidos,  PromoDesc,  PromoCode,  NoEmpleado,  Vendedor)
                                    VALUES          	(@Estacion, @Gasolinera, @Transaccion, @Descuento, @CardNumber, @NombreCliente, @litrosRedimidos, @PromoDesc, @PromoCode, @NoEmpleado, @Vendedor)";

                    var parameters = new { Estacion = _estacion, Gasolinera = _gasolinera, Transaccion = transaccion, Descuento = descuento, CardNumber = cardNumber, NombreCliente = nombreCliente, litrosRedimidos = litrosRedimidos, PromoDesc, PromoCode = promoCode, NoEmpleado = noEmpleado, Vendedor = vendedor };
                    var affectedRecords = connection.ExecuteNonQuery(sql, parameters);
                    if (affectedRecords == 0)
                    {
                        Result.Success = false;
                        Result.Error = "";
                        Result.Message = "No se inserto el registro";
                    }
                    else
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = "Registro agregado";
                    }
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<DescuentoDTO>> GetDescuento(string transaccion)
        {
            ResultSingle<DescuentoDTO> Result = new ResultSingle<DescuentoDTO>();
            try
            {
                string sql = $"SELECT * FROM HTI_Descuentos WHERE Estacion = {_estacion} AND Transaccion = {transaccion}";

                using var connection = new SqlConnection(_connectionStringCG);
                var req = await connection.ExecuteQueryAsync<DescuentoDTO>(sql);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = "No se encontraron registros";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Registro encontrado";
                    Result.Data = req.FirstOrDefault();
                }
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
