using ADDESAPI.Core;
using ADDESAPI.Core.GetnetCQRS;
using ADDESAPI.Core.TipoCambioDTO;
using ADDESAPI.Core.TipoCambioDTO.DTO;
using Microsoft.Extensions.Configuration;
using RepoDb;
using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Infrastructure
{
    public class TipoCambioResource : ITipoCambioResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        //public readonly string _connectionStringAddes;
        public readonly int _gasolinera;
        public readonly int _estacion;
        private readonly string _urlAddes;
        private readonly string _userAddes;
        private readonly string _pwdAddes;
        public readonly string _apiTC;
        public TipoCambioResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            //_connectionStringAddes = _configuration["ConnectionStrings:AddesConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
            _urlAddes = _configuration["ADDES:URL"];
            _userAddes = _configuration["ADDES:UsrAddes"];
            _pwdAddes = _configuration["ADDES:PwdAddes"];
            _apiTC = _configuration["ADDES:ApiTipoCambio"];
        }
        //public async Task<ResultSingle<vTipoCambio>> GetTipoCambio(string fecha)
        //{
        //    ResultSingle<vTipoCambio> Result = new ResultSingle<vTipoCambio>();
        //    try
        //    {
        //        string sql = $"SELECT TOP 1 Id, Fecha, TC, Estacion FROM vTipoCambio WHERE Estacion = {_estacion} AND Fecha = '{fecha}' ORDER BY Fecha DESC";
        //        using var connection = new SqlConnection(_connectionStringAddes);
        //        var req = await connection.ExecuteQueryAsync<vTipoCambio>(sql);

        //        if (req == null || req.Count() == 0)
        //        {
        //            Result.Success = false;
        //            Result.Error = "Error";
        //            Result.Message = "No se encotro el tipo de cambio";
        //        }
        //        else
        //        {
        //            Result.Success = true;
        //            Result.Error = "";
        //            Result.Message = "Tipo de Cambio encontrado";
        //            Result.Data = req.FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Success = false;
        //        Result.Error = "Error al obtener el tipo de cambio";
        //        Result.Message = ex.Message;
        //    }
        //    return Result;
        //}
        public async Task<ResultSingle<vTipoCambio>> GetTipoCambio(string fecha)
        {
            ResultSingle<vTipoCambio> Result = new ResultSingle<vTipoCambio>();
            try
            {
                var request = new RestRequest(_apiTC, Method.Post);

                var options = new RestClientOptions(_urlAddes)
                {
                    Authenticator = new HttpBasicAuthenticator(_userAddes, _pwdAddes)
                };
                var client = new RestClient(options);

                var Data = new
                {
                    Fecha = fecha,
                    Estacion = _estacion
                };

                request.AddJsonBody(new { Data = Data });
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultADDES<vTipoCambio>>(response.Content);

                    Result.Success = r.Success;
                    if (!r.Success)
                    {
                        Result.Error = $"Error al obtener el TC {r.Error}";
                        Result.Message = $". Respuesta: {r.Message}";
                    }
                    else
                    {
                        Result.Error = r.Error;
                        Result.Message = r.Message;
                        Result.Data = r.response;
                    }
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error al obtener el TC";
                    Result.Message = $"Error de conexión. {_urlAddes} EstatusCode: {response.StatusCode} {response.StatusDescription}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al obtener el tipo de cambio";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
