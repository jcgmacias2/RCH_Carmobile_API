using ADDESAPI.Core;
using ADDESAPI.Core.DespachosCQRS;
using ADDESAPI.Core.DespachosCQRS.DTO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RepoDb;
using RepoDb.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Infrastructure
{
    public class DespachosResource : IDespachosResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly int _gasolinera;

        public DespachosResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
        }
        //public async Task<ResultMultiple<vDespachos>> GetDespachos(int bomba)
        //{
        //    ResultMultiple<vDespachos> Result = new ResultMultiple<vDespachos>();
        //    try
        //    {
        //        string sql = $"SELECT TOP 3 Transaccion, Gasolinera, Turno, FechaCG, Fecha, Hora, Bomba, TipoPago, IdTipoPago, Total, Cliente, UUID, Descripcion, Producto, lognew, PermisoCRE FROM vDespachos WHERE Gasolinera = {_gasolinera} AND Bomba = {bomba} AND SUBSTRING(CAST(Transaccion AS VARCHAR(25)), LEN(Transaccion), 1) = '0' /*AND Total > 0*/ ORDER BY lognew DESC";
        //        using var connection = new SqlConnection(_connectionString);
        //        var req = await connection.ExecuteQueryAsync<vDespachos>(sql);

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
        public async Task<ResultMultiple<DespachoAppDTO>> GetDespachos(int bomba)
        {
            ResultMultiple<DespachoAppDTO> Result = new ResultMultiple<DespachoAppDTO>();
            try
            {
                string sql = $"SELECT TOP 3 Despacho 'Transaccion', Gasolinera, Turno, Fecha, Hora, Bomba, TipoPago, IdTipoPago, SUM(Total) Total, Despacho FROM vDespachos WHERE Gasolinera = {_gasolinera} AND Bomba = {bomba} AND SUBSTRING(CAST(Transaccion AS VARCHAR(25)), LEN(Transaccion), 1) = '0' /*AND Total > 0*/ GROUP BY Despacho, Gasolinera, Turno, Fecha, Hora, Bomba, TipoPago, IdTipoPago, lognew ORDER BY lognew DESC";
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.ExecuteQueryAsync<DespachoAppDTO>(sql);

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
        public async Task<ResultMultiple<DespachoDetalleAppDTO>> GetDespachosApp(string despachos)
        {
            ResultMultiple<DespachoDetalleAppDTO> Result = new ResultMultiple<DespachoDetalleAppDTO>();
            try
            {
                string sql = $"SELECT CAST(Transaccion AS VARCHAR(20)) 'Transaccion', Producto, Cantidad, Descripcion, Precio, Total, CAST(Despacho AS VARCHAR(20)) 'Despacho' FROM vDespachos WHERE Gasolinera = {_gasolinera} AND Despacho IN({despachos}) ";
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.ExecuteQueryAsync<DespachoDetalleAppDTO>(sql);

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
                using (var connection = new SqlConnection(_connectionString))
                {
                    var sql = $"UPDATE [CG].dbo.Despachos SET tiptrn = {tipoPago}, logfch = GETDATE() WHERE codgas = {_gasolinera} AND nrotrn = {despacho}";
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
    }
}
