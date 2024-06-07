using ADDESAPI.Core;
using ADDESAPI.Core.DespachosCQRS;
using ADDESAPI.Core.DespachosCQRS.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RepoDb;
using RepoDb.Enumerations;
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
    public class DespachosResource : IDespachosResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly string _connectionStringCG;
        public readonly int _gasolinera;
        public readonly string _urlADDES;
        public readonly string _apiRedemption;
        public readonly string _apiRewardRedemption;
        public readonly string _userAddes;
        public readonly string _pwdAddes;

        public DespachosResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _connectionStringCG = _configuration["ConnectionStrings:CGConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
            _urlADDES = _configuration["ADDES:URL"];
            _apiRedemption = _configuration["ADDES:ApiRedemption"];
            _apiRewardRedemption = _configuration["ADDES:ApiRewardRedemption"];
            _userAddes = _configuration["ADDES:UsrAddes"];
            _pwdAddes = _configuration["ADDES:PwdAddes"];
        }
        public async Task<ResultMultiple<DespachoAppDTO>> GetDespachos(int bomba)
        {
            ResultMultiple<DespachoAppDTO> Result = new ResultMultiple<DespachoAppDTO>();
            try
            {
                string sql = @"
                SELECT x.*
	                , (SELECT TOP 1 Descripcion FROM vDespachos p WHERE x.Transaccion = p.Despacho AND Descripcion != '' ORDER BY Transaccion) 'Descripcion'
	                , (SELECT COUNT(*) FROM vDespachos p WHERE Producto NOT IN (62, 63, 64) AND x.Transaccion = p.Despacho) 'Productos'
                FROM (
	                SELECT TOP 3 Despacho 'Transaccion'
		                , Gasolinera
		                , Turno
		                , Fecha
		                , Hora
		                , Bomba
		                , TipoPago
		                , IdTipoPago
		                , SUM(Total) Total
	                FROM vDespachos d 
	                WHERE Gasolinera = @Gasolinera AND Bomba = @Bomba 
	                GROUP BY Despacho, Gasolinera, Turno, Fecha, Hora, Bomba, TipoPago, IdTipoPago,  FechaHora 
	                ORDER BY FechaHora DESC
                ) x
                ORDER BY Transaccion DESC";
                var parameters = new { Gasolinera = _gasolinera, Bomba = bomba };
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.ExecuteQueryAsync<DespachoAppDTO>(sql, parameters);

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
        //Este no jlo en la mesa
        //public async Task<ResultMultiple<DespachoAppDTO>> GetDespachos(int bomba)
        //{
        //    ResultMultiple<DespachoAppDTO> Result = new ResultMultiple<DespachoAppDTO>();
        //    try
        //    {
        //        string sql = @"
        //        SELECT TOP 3 Despacho 'Transaccion'
        //         , Gasolinera
        //         , Turno
        //         , Fecha
        //         , Hora
        //         , Bomba
        //         , TipoPago
        //         , IdTipoPago
        //         , (SELECT TOP 1 Descripcion FROM vDespachos p WHERE d.Despacho = p.Despacho AND Descripcion != '' ORDER BY Transaccion) 'Descripcion'
        //         , SUM(Total) Total
        //         , (SELECT COUNT(*) FROM vDespachos p WHERE Producto NOT IN (62, 63, 64) AND d.Despacho = p.Despacho) 'Productos' 
        //        FROM vDespachos d 
        //        WHERE Gasolinera = @Gasolinera AND Bomba = @Bomba 
        //        GROUP BY Despacho, Gasolinera, Turno, Fecha, Hora, Bomba, TipoPago, IdTipoPago,  FechaHora 
        //        ORDER BY FechaHora DESC";
        //        var parameters = new { Gasolinera = _gasolinera, Bomba = bomba};
        //        using var connection = new SqlConnection(_connectionString);
        //        var req = await connection.ExecuteQueryAsync<DespachoAppDTO>(sql, parameters);

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
        //            Result.Message = "Registros encontrados";
        //            Result.Data = req.ToList();
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
        //Se dejo de utilizar el 07 Marzo 2024 para agregar la descripcion y la cantidad de productos
        //public async Task<ResultMultiple<DespachoAppDTO>> GetDespachos(int bomba)
        //{
        //    ResultMultiple<DespachoAppDTO> Result = new ResultMultiple<DespachoAppDTO>();
        //    try
        //    {
        //        string sql = $"SELECT TOP 3 Despacho 'Transaccion', Gasolinera, Turno, Fecha, Hora, Bomba, TipoPago, IdTipoPago, SUM(Total) Total, (SELECT COUNT(*) FROM vDespachos p WHERE Producto NOT IN (62, 63, 64) AND d.Despacho = p.Despacho) 'Productos' FROM vDespachos d WHERE Gasolinera = {_gasolinera} AND Bomba = {bomba} GROUP BY Despacho, Gasolinera, Turno, Fecha, Hora, Bomba, TipoPago, IdTipoPago,  FechaHora ORDER BY FechaHora DESC";
        //        using var connection = new SqlConnection(_connectionString);
        //        var req = await connection.ExecuteQueryAsync<DespachoAppDTO>(sql);

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
        //            Result.Message = "Registros encontrados";
        //            Result.Data = req.ToList();
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
        //public async Task<ResultMultiple<DespachoDetalleAppDTO>> GetDespachosApp(string despachos)
        //{
        //    ResultMultiple<DespachoDetalleAppDTO> Result = new ResultMultiple<DespachoDetalleAppDTO>();
        //    try
        //    {
        //        string sql = $"SELECT CAST(Transaccion AS VARCHAR(20)) 'Transaccion', Producto, Cantidad, Descripcion, Precio, Total, CAST(Despacho AS VARCHAR(20)) 'Despacho' FROM vDespachos WHERE Gasolinera = {_gasolinera} AND Despacho IN({despachos}) ";
        //        using var connection = new SqlConnection(_connectionString);
        //        var req = await connection.ExecuteQueryAsync<DespachoDetalleAppDTO>(sql);

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
        //            Result.Message = "Registros encontrados";
        //            Result.Data = req.ToList();
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
        public async Task<ResultSingle<vDespachos>> GetDespacho(int despacho)
        {
            ResultSingle<vDespachos> Result = new ResultSingle<vDespachos>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<vDespachos>(r => r.Gasolinera == _gasolinera && r.Despacho == despacho);

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
        public async Task<ResultMultiple<vDespachos>> GetDespachoDetalle(int despacho)
        {
            ResultMultiple<vDespachos> Result = new ResultMultiple<vDespachos>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<vDespachos>(r => r.Gasolinera == _gasolinera && r.Despacho == despacho);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = "No se encontro el detalle del despacho";
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
        public async Task<Result> SetTipoPago(int despacho, int tipoPago)
        {
            Result Result = new Result();
            try
            {
                string d = despacho.ToString().Substring(0, despacho.ToString().Length -1);
                using (var connection = new SqlConnection(_connectionString))
                {
                    string sql = $"UPDATE [CG].dbo.Despachos SET tiptrn = {tipoPago}, logfch = GETDATE() WHERE codgas = {_gasolinera} AND SUBSTRING(CAST(nrotrn AS VARCHAR(25)), 1, LEN(nrotrn) -1) = {d}";
                    var affectedRecords = connection.ExecuteNonQuery(sql);
                    if (affectedRecords == 0)
                    {
                        Result.Success = false;
                        Result.Error = "";
                        Result.Message = "No se actualizo el despacho";
                    }
                    else
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = "Despacho actualizado";
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
        public async Task<ResultSingle<RedemptionDTO>> Redemption(RedemptionReq req)
        {
            ResultSingle<RedemptionDTO> Result = new ResultSingle<RedemptionDTO>();
            try
            {

                var request = new RestRequest(_apiRedemption, Method.Post);

                var options = new RestClientOptions(_urlADDES)
                {
                    Authenticator = new HttpBasicAuthenticator(_userAddes, _pwdAddes)
                };
                var client = new RestClient(options);
                string jsonString = System.Text.Json.JsonSerializer.Serialize(req);

                request.AddJsonBody(new { data = req});
                request.RequestFormat = DataFormat.Json;
                var response = client.Execute<string>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultADDES<RedemptionDTO>>(response.Content);

                    Result.Success = r.Success;
                    if (!r.Success)
                    {
                        Result.Error = $"Error al redimir";
                        Result.Message = $"{r.Message}";
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
                    Result.Error = "Error al Redimir";
                    Result.Message = $"{response.Content}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al Redimir";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<RedemptionDTO>> RewardRedemption(RedemptionReq req)
        {
            ResultSingle<RedemptionDTO> Result = new ResultSingle<RedemptionDTO>();
            try
            {

                var request = new RestRequest(_apiRewardRedemption, Method.Post);

                var options = new RestClientOptions(_urlADDES)
                {
                    Authenticator = new HttpBasicAuthenticator(_userAddes, _pwdAddes)
                };
                var client = new RestClient(options);
                string jsonString = System.Text.Json.JsonSerializer.Serialize(req);

                request.AddJsonBody(new { data = req });
                request.RequestFormat = DataFormat.Json;
                var response = client.Execute<string>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultADDES<RedemptionDTO>>(response.Content);

                    Result.Success = r.Success;
                    if (!r.Success)
                    {
                        Result.Error = $"Error al redimir";
                        Result.Message = $"{r.Message}";
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
                    Result.Error = "Error al Redimir";
                    Result.Message = $"{response.Content}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al Redimir";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
