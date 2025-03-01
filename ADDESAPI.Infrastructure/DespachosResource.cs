using ADDESAPI.Core;
using ADDESAPI.Core.DespachosCQRS;
using ADDESAPI.Core.DespachosCQRS.DTO;
using ADDESAPI.Core.EstacionCQRS.DTO;
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
        public readonly int _estacion;
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
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
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
                if (_estacion == 6611)
                {
                    using var connection = new SqlConnection(_connectionString);
                    {
                        var result = connection.ExecuteQuery<DespachoAppDTO>("EXEC [dbo].[SP_DESPACHOS] @Gasolinera, @Bomba",
                                            new { Gasolinera = _gasolinera, Bomba = bomba }
                                            ).ToList();
                        if (result != null && result.Any())
                        {
                            Result.Success = true;
                            Result.Data = result.ToList();
                        }
                        else
                        {
                            Result.Success = false;
                            Result.Message = "No se encontraron registros";
                        }
                    }
                }
                else
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
                            , ISNULL(CardNumber, '') 'CardNumber'
		                    , ISNULL(NombreCliente, '') 'NombreCliente'
		                    , ISNULL(Descuento, 0) 'Descuento'
		                    , ISNULL(litrosRedimidos, 0) 'litrosRedimidos'
		                    , ISNULL(PromoDesc, '') 'PromoDesc'
		                    , ISNULL(PromoCode, '') 'PromoCode'
	                    FROM vDespachos d 
	                    WHERE Gasolinera = @Gasolinera AND Bomba = @Bomba 
	                    GROUP BY Despacho, Gasolinera, Turno, Fecha, Hora, Bomba, TipoPago, IdTipoPago, FechaHora, CardNumber, NombreCliente, Descuento, litrosRedimidos, PromoDesc, PromoCode
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

                if (_estacion == 6611)
                {
                    string sql = GetQueryBlas(despacho.ToString());
                    var req = await connection.ExecuteQueryAsync<vDespachos>(sql);

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
                else
                {
                    string sql = $"SELECT * FROM vDespachos WHERE Gasolinera = {_gasolinera} AND Despacho = {despacho}";
                    var req = await connection.ExecuteQueryAsync<vDespachos>(sql);

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
                if (_estacion == 6611)
                {
                    string sql = GetQueryBlas(despacho.ToString());
                    var req = await connection.ExecuteQueryAsync<vDespachos>(sql);

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
                else
                {
                    string sql = $"SELECT * FROM vDespachos WHERE Gasolinera = {_gasolinera} AND Despacho = {despacho}";
                    var req = await connection.ExecuteQueryAsync<vDespachos>(sql);

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

        public string GetQueryBlas(string despacho)
        {
            string sql = $@"SELECT d.nrotrn 'Transaccion' 
	            , d.codgas 'Gasolinera'
	            , e.est_id 'NoEstacion'
	            , e.est_nombre 'Estacion'
	            , d.nrotur 'Turno'	
                , d.fchtrn 'FechaCG'
	            , d.fchcor 'FchCor'
	            , CAST(CAST(d.fchtrn as DATETIME) -1 AS DATE) 'Fecha'
	            , CAST(CAST(d.fchcor as DATETIME) -1 AS DATE) 'FechaCorte'
	            , SUBSTRING(CONVERT(CHAR(5), d.hratrn + 10000), 2, 2) + ':' + SUBSTRING(CONVERT(CHAR(5), d.hratrn + 10000), 4, 2) 'Hora'
	            , CAST(CONVERT(VARCHAR, CAST(d.fchtrn as DATETIME) -1, 23) AS VARCHAR(10)) + ' ' + SUBSTRING(CONVERT(CHAR(5), d.hratrn + 10000), 2, 2) + ':' + SUBSTRING(CONVERT(CHAR(5), d.hratrn + 10000), 4, 2) + ':00'  'FechaHora'
	            , d.nrobom 'Bomba'
	            , b.Isla 'IslaID'
	            , CASE WHEN d.tiptrn IN(0, 48, 49) THEN 'Efectivo' ELSE f.Descripcion END 'TipoPago'
	            , CASE WHEN d.tiptrn IN(0, 48, 49) THEN 0 ELSE d.tiptrn END  'IdTipoPago'
	            , CASE WHEN d.tiptrn IN(0, 48, 49) THEN '01' ELSE f.ClaveSAT END 'FormaPagoSAT'
	            , d.can 'Cantidad'
	            , d.pre 'Precio'
                , d.mto 'Total'
	            , d.codcli 'Cliente'
	            , ISNULL(d.satuid, '') 'UUID' 
	            , p.cod 'Producto'
	            , RTRIM(LTRIM(p.den)) 'Descripcion'
	            , p.uni 'Unidad'
	            , d.lognew 'lognew' 
	            , g.nropcc 'PermisoCRE'
	            , SUBSTRING(CAST(d.nrotrn AS VARCHAR(25)), 1, LEN(d.nrotrn) -1) 'Despacho'
	            , CASE WHEN p.cod in (1, 2, 16, 62, 63, 64) THEN 'LTR' ELSE 'H87' END 'ClaveUnidad' 
	            , p.codsat 'ClaveProdServ'
	            , est_rfc 'RFC'
	            , RazonSocial 'RazonSocial'
	            , est_direccion 'Direccion'
	            , e.RazonSocialDom 'DomicilioFiscal'
	            , d.codres 'NoEmpleado'
	            , c.Nombre 'Vendedor'
	            , x.CardNumber
	            , x.NombreCliente
	            , x.Descuento
	            , x.litrosRedimidos
	            , x.PromoCode
	            , x.PromoDesc
            FROM [CG].dbo.Despachos d (NOLOCK)
	            LEFT JOIN [CG].dbo.Productos p(NOLOCK) ON d.codprd = p.cod
	            LEFT JOIN [CG].dbo.Gasolineras g (NOLOCK) ON d.codgas = g.cod
	            LEFT JOIN [CG].dbo.HTI_Descuentos x(NOLOCK) ON d.nrotrn = x.Transaccion
	            LEFT JOIN [ADDES].dbo.[FormaPago] f (NOLOCK) ON f.ClaveCG = d.tiptrn
	            LEFT JOIN [ADDES].dbo.[Estaciones] e(NOLOCK) ON d.codgas = e.est_codigo_gas
	            LEFT JOIN [ADDES].dbo.[vBombas] b(NOLOCK) ON d.nrobom = b.Numero AND e.est_id = b.Estacion
	            LEFT JOIN [ADDES].dbo.[vColaborador] c(NOLOCK) ON d.codres = c.NumeroVentuk
            WHERE d.codgas = {_gasolinera} AND SUBSTRING(CAST(d.nrotrn AS VARCHAR(25)), 1, LEN(d.nrotrn) -1) = {despacho}";
            return sql;
        }
    }
}
