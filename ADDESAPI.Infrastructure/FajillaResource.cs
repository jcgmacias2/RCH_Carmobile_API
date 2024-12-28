using ADDESAPI.Core;
using ADDESAPI.Core.FajillaCQRS;
using ADDESAPI.Core.FajillaCQRS.DTO;
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
    public class FajillaResource : IFajillaResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly string _connectionStringAddes; 
        public readonly int _gasolinera;
        public readonly int _estacion;
        private readonly string _urlAddes;
        private readonly string _userAddes;
        private readonly string _pwdAddes;
        public readonly string _apiGetFajillas;
        public FajillaResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _connectionStringAddes = _configuration["ConnectionStrings:AddesConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
            _urlAddes = _configuration["ADDES:URL"];
            _userAddes = _configuration["ADDES:UsrAddes"];
            _pwdAddes = _configuration["ADDES:PwdAddes"];
            _apiGetFajillas = _configuration["ADDES:ApiGetFajillas"];
        }
        //public async Task<ResultMultiple<vFajillas>> GetFajillasColaborador(string fecha, int noEmpleado, int turno)
        //{
        //    ResultMultiple<vFajillas> Result = new ResultMultiple<vFajillas>();
        //    try
        //    {
        //        vFajillas Fajillas = new vFajillas();

        //        string sql = $"SELECT * FROM vFajillas WHERE CAST(Fecha AS DATE) = '{fecha}' AND NoEmpleado = {noEmpleado} AND NoEstacion = {_estacion} AND Turno = {turno}";
        //        using var connection = new SqlConnection(_connectionStringAddes);
        //        var request = await connection.ExecuteQueryAsync<vFajillas>(sql);

        //        if (request == null || request.Count() == 0)
        //        {
        //            Result.Success = false;
        //            Result.Error = "Error";
        //            Result.Message = "No se encotraron fajillas";
        //        }
        //        else
        //        {
        //            Result.Success = true;
        //            Result.Error = "";
        //            Result.Message = "";
        //            Result.Data = request.ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Success = false;
        //        Result.Error = "Error al obtener las fajillas";
        //        Result.Message = ex.Message;
        //    }
        //    return Result;
        //}
        public async Task<ResultMultiple<vFajillas>> GetFajillasColaborador(string fecha, int noEmpleado, int turno)
        {
            ResultMultiple<vFajillas> Result = new ResultMultiple<vFajillas>();
            try
            {
                var request = new RestRequest(_apiGetFajillas, Method.Post);

                var options = new RestClientOptions(_urlAddes)
                {
                    Authenticator = new HttpBasicAuthenticator(_userAddes, _pwdAddes)
                };
                var client = new RestClient(options);

                var Data = new
                {
                    Fecha = fecha,
                    Estacion = _estacion,
                    NoEmpleado = noEmpleado,
                    Turno = turno
                };

                request.AddJsonBody(new { Data = Data });
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultADDES<List<vFajillas>>>(response.Content);

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
                Result.Error = "Error al obtener las fajillas";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
