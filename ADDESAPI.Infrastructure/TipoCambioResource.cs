using ADDESAPI.Core;
using ADDESAPI.Core.TipoCambioDTO;
using ADDESAPI.Core.TipoCambioDTO.DTO;
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
    public class TipoCambioResource : ITipoCambioResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly string _connectionStringAddes;
        public readonly int _gasolinera;
        public readonly int _estacion;
        public TipoCambioResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _connectionStringAddes = _configuration["ConnectionStrings:AddesConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
        }
        public async Task<ResultSingle<vTipoCambio>> GetTipoCambio(string fecha)
        {
            ResultSingle<vTipoCambio> Result = new ResultSingle<vTipoCambio>();
            try
            {
                string sql = $"SELECT TOP 1 Id, Fecha, TC, Estacion FROM vTipoCambio WHERE Estacion = {_estacion} AND Fecha = '{fecha}' ORDER BY Fecha DESC";
                using var connection = new SqlConnection(_connectionStringAddes);
                var req = await connection.ExecuteQueryAsync<vTipoCambio>(sql);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = "No se encotro el tipo de cambio";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Tipo de Cambio encontrado";
                    Result.Data = req.FirstOrDefault();
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
