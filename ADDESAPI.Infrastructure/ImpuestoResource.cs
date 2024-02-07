using ADDESAPI.Core;
using ADDESAPI.Core.ImpuestoCQRS;
using ADDESAPI.Core.ImpuestoCQRS.DTO;
using Microsoft.Extensions.Configuration;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Infrastructure
{
    public class ImpuestoResource : IImpuestoResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly int _gasolinera;

        public ImpuestoResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
        }
        public async Task<ResultSingle<vImpuesto>> GetImpuestoProducto(int fecha, int producto)
        {
            ResultSingle<vImpuesto> Result = new ResultSingle<vImpuesto>();
            try
            {
                string sql = $"SELECT TOP 1 Gasolinera, FechaCG, Producto, TasaIVA, CuotaIEPS, Precio FROM vImpuesto WHERE Gasolinera = {_gasolinera} AND FechaCG <= {fecha} AND Producto = {producto} ORDER BY FechaCG DESC";
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.ExecuteQueryAsync<vImpuesto>(sql);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = $"No se encontraron impuestos para el producto {producto}";
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
        public async Task<ResultSingle<vImpuesto>> GetImpuestoProducto(int producto)
        {
            ResultSingle<vImpuesto> Result = new ResultSingle<vImpuesto>();
            try
            {
                string sql = $"SELECT TOP 1 Gasolinera, FechaCG, Producto, TasaIVA, CuotaIEPS, Precio FROM vImpuesto WHERE Gasolinera = {_gasolinera} AND Producto = {producto} ORDER BY FechaCG DESC";
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.ExecuteQueryAsync<vImpuesto>(sql);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = $"No se encontraron impuestos para el producto {producto}";
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
    }
}
