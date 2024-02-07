using ADDESAPI.Core;
using ADDESAPI.Core.ProducoCQRS;
using ADDESAPI.Core.ProducoCQRS.DTO;
using Microsoft.Extensions.Configuration;
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
    public class ProductoResource : IProductoResource
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _url;
        private readonly string _apiFamilias;
        private readonly string _apiProductosFamilia;
        private readonly string _apiAddProductosTicket;
        private readonly string _apiAddProductos; 
        private readonly string _apiProductos;
        public ProductoResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _url = _configuration["GT:URL"];
            _apiFamilias = _configuration["GT:ApiFamilias"];
            _apiProductosFamilia = _configuration["GT:ApiProductosFamilia"];
            _apiAddProductosTicket = _configuration["GT:ApiAddProductsTicket"];
            _apiAddProductos = _configuration["GT:ApiAddProducts"];
            _apiProductos = _configuration["GT:ApiProductos"];            
        }
        public async Task<ResultMultiple<FamiliaGT>> GetFamilias(string token)
        {
            ResultMultiple<FamiliaGT> Result = new ResultMultiple<FamiliaGT>();
            try
            {
                var request = new RestRequest(_apiFamilias, Method.Get);
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
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<List<FamiliaGT>>(response.Content);

                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = $"";
                    Result.Data = r;

                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"Error al obtener las Familias de productos {_apiFamilias}. {_url} EstatusCode: {response.StatusCode } {response.StatusDescription}";
                }

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al obtener las Familias de productos {_apiFamilias} Excepcion: {ex.Message}";
            }
            return Result;
        }
        public async Task<ResultMultiple<ProductoGT>> GetProductosFamilia(string token, int familia)
        {
            ResultMultiple<ProductoGT> Result = new ResultMultiple<ProductoGT>();
            string _api = $"{_apiProductosFamilia}/{familia}?hab=1";
            try
            {
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
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProductoGT>>(response.Content);

                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = $"";
                    Result.Data = r;

                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"Error al obtener los productos {_api}. {_url} EstatusCode: {response.StatusCode } {response.StatusDescription}";
                }

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al obtener los productos {_api} Excepcion: {ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<vProductos>> GetProductoCB(string token, string codigo)
        {
            ResultSingle<vProductos> Result = new ResultSingle<vProductos>();

            try
            {
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<vProductos>(r => r.CodigoBarras == codigo);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = "No se encontro el producto";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Producto encontrado";
                    Result.Data = req.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al obtener el producto Excepcion: {ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<TicketGT>> SetProductoTicket(string token, int bomba, string jsonProductos)
        {
            ResultSingle<TicketGT> Result = new ResultSingle<TicketGT>();
            string _api = $"{_apiAddProductosTicket}/{bomba}";
            try
            {
                var request = new RestRequest(_api, Method.Post);
                var authenticator = new JwtAuthenticator(token);
                var options = new RestClientOptions(_url)
                {
                    Authenticator = authenticator
                };
                var client = new RestClient(options);
                request.AddJsonBody(jsonProductos);
                request.RequestFormat = DataFormat.Json;
                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketGT>(response.Content);

                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = $"";
                    Result.Data = r;
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"Error al guardar los productos.{response.Content} URL {_url} EstatusCode: {response.StatusCode } {response.StatusDescription}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al obtener los productos {_api} Excepcion: {ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<TicketGT>> SetProducto(string token, int bomba, string jsonProductos)
        {
            ResultSingle<TicketGT> Result = new ResultSingle<TicketGT>();
            string _api = $"{_apiAddProductos}/{bomba}";
            try
            {
                var request = new RestRequest(_api, Method.Post);
                var authenticator = new JwtAuthenticator(token);
                var options = new RestClientOptions(_url)
                {
                    Authenticator = authenticator
                };
                var client = new RestClient(options);
                request.AddJsonBody(jsonProductos);
                request.RequestFormat = DataFormat.Json;
                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<TicketGT>(response.Content);

                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = $"";
                    Result.Data = r;
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"Error al guardar los productos. {_url} EstatusCode: {response.StatusCode } {response.Content}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al obtener los productos {_api} Excepcion: {ex.Message}";
            }
            return Result;
        }
    }
}
