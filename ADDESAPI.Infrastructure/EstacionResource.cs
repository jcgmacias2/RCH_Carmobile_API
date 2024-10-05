using ADDESAPI.Core;
using ADDESAPI.Core.EstacionCQRS;
using ADDESAPI.Core.EstacionCQRS.DTO;
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
    public class EstacionResource : IEstacionResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly int _estacion;
        public readonly int _gasolinera;
        public readonly string _apiSetDispensarioCambio;
        public readonly string _urlAddes;
        private readonly string _userAddes;
        private readonly string _pwdAddes;
        public readonly string _licenciaGetnet;

        public EstacionResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
            _urlAddes = _configuration["ADDES:URL"];
            _userAddes = _configuration["ADDES:UsrAddes"];
            _pwdAddes = _configuration["ADDES:PwdAddes"];
            _apiSetDispensarioCambio = _configuration["ADDES:ApiSetDispensarioCambio"];
            _licenciaGetnet = _configuration["Settings:LicenciaGetnet"];
        }
        public async Task<ResultMultiple<vBombas>> GetBombas()
        {
            ResultMultiple<vBombas> Result = new ResultMultiple<vBombas>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<vBombas>(r => r.Estacion == _estacion);

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
                    Result.Message = "Registros encontrados";
                    Result.Data = req.ToList();
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
        public async Task<ResultMultiple<EstacionTanques>> GetTanques()
        {
            ResultMultiple<EstacionTanques> Result = new ResultMultiple<EstacionTanques>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<EstacionTanques>(r => r.Estacion == _estacion);

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
                    Result.Message = "Registros encontrados";
                    Result.Data = req.ToList();
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
        public async Task<ResultSingle<vGasolinera>> GetGasolinera()
        {
            ResultSingle<vGasolinera> Result = new ResultSingle<vGasolinera>();
            try
            {
                string sql = $"SELECT Codigo, Nombre, Denominacion, TurnoActual, CodigoExterno, Clave, PermisoCRE, CP, Fecha FROM vGasolinera WHERE Codigo = {_gasolinera}";
                using var connection = new SqlConnection(_connectionString);
                //var req = await connection.QueryAsync<vGasolinera>(r => r.Codigo == _gasolinera);
                var req = await connection.ExecuteQueryAsync<vGasolinera>(sql);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"No se encontro la gasolinera con codigo {_gasolinera}";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Registros encontrados";
                    Result.Data = req.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al obtener la gasolinera";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<Result> SetDispensarioCambios(int bomba, string usuario, int modo, string comentarios)
        {
            Result Result = new Result();
            try
            {

                var request = new RestRequest(_apiSetDispensarioCambio, Method.Post);

                var options = new RestClientOptions(_urlAddes)
                {
                    Authenticator = new HttpBasicAuthenticator(_userAddes, _pwdAddes)
                };
                var client = new RestClient(options);

                var Data = new
                {
                    Estacion = _estacion,
                    Bomba = bomba,
                    Usuario = usuario,
                    Modo = modo,
                    Comentarios = comentarios
                };

                request.AddJsonBody(new { Data = Data });
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultADDES>(response.Content);

                    Result.Success = r.Success;
                    if (!r.Success)
                    {
                        Result.Error = $"Error al enviar la bitacora de cambios {r.Error}";
                        Result.Message = $". Respuesta: {r.Message}";
                    }
                    else
                    {
                        Result.Error = r.Error;
                        Result.Message = r.Message;
                    }
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error al enviar la bitacora de cambios";
                    Result.Message = $"Error de conexión. {_urlAddes} EstatusCode: {response.StatusCode } {response.StatusDescription}";
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
        public async Task<ResultSingle<vEstacion>> GetEstacion()
        {
            ResultSingle<vEstacion> Result = new ResultSingle<vEstacion>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<vEstacion>(r => r.NoEstacion == _estacion);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"No se encontro la estacion con codigo {_estacion}";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Registros encontrados";
                    Result.Data = req.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al obtener la estacion";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultMultiple<FormaPago>> GetFormasPago()
        {
            ResultMultiple<FormaPago> Result = new ResultMultiple<FormaPago>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<FormaPago>(r => r.AplicaEstacion == true && r.ClaveCG != 48 && r.ClaveCG != 65 && r.ClaveCG != 74);

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
                    Result.Message = "Registros encontrados";
                    Result.Data = req.ToList();
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
        public async Task<ResultSingle<string>> GetLicenciaGetnet()
        {
            ResultSingle<string> Result = new ResultSingle<string>();
            try
            {


                Result.Success = true;
                Result.Error = "";
                Result.Message = "";
                Result.Data = _licenciaGetnet;

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = ex.Message;
            }
            return Result;
        }
        //public async Task<ResultSingle<FechaActualDTO>> GetFecha()
        //{
        //    ResultSingle<FechaActualDTO> Result = new ResultSingle<FechaActualDTO>();
        //    try
        //    {
        //        string sql = $"SELECT CASE WHEN g.turact BETWEEN 11 AND 19 THEN 11 WHEN g.turact BETWEEN 21 AND 29 THEN 21 WHEN g.turact BETWEEN 31 AND 39 THEN 31 WHEN g.turact BETWEEN 41 AND 49 THEN 31 END 'Turno',  CAST(CAST(g.fchact as DATETIME) -1 AS DATE) 'Fecha' FROM [CG].dbo.Gasolineras g(NOLOCK) WHERE cod = {_gasolinera}";
        //        using var connection = new SqlConnection(_connectionString);
        //        var req = await connection.ExecuteQueryAsync<FechaActualDTO>(sql);

        //        if (req == null || req.Count() == 0)
        //        {
        //            Result.Success = false;
        //            Result.Error = "Error";
        //            Result.Message = $"No se pudo obtener la fecha actual para la gasolinera {_gasolinera}";
        //        }
        //        else
        //        {
        //            Result.Success = true;
        //            Result.Error = "";
        //            Result.Message = "Registros encontrados";
        //            Result.Data = req.FirstOrDefault();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Success = false;
        //        Result.Error = "Error al obtener la fecha actual";
        //        Result.Message = ex.Message;
        //    }
        //    return Result;
        //}
    }
}
