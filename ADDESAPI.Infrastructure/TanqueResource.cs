using ADDESAPI.Core;
using ADDESAPI.Core.EstacionCQRS.DTO;
using ADDESAPI.Core.TanqueCQRS;
using ADDESAPI.Core.TanqueCQRS.DTO;
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
    public class TanqueResource : ITanqueResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly int _estacion;
        public readonly int _gasolinera;
        public TanqueResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
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
                    Result.Error = "Error al obtener tanques";
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
                Result.Error = "Error al obtener tanques";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<LecturaDTO>> GetUltimaLectura(int tanque)
        {
            ResultSingle<LecturaDTO> Result = new ResultSingle<LecturaDTO>();
            try
            {
                string sql = $"SELECT TOP 1 * FROM vMovimientosTanque WHERE Gasolinera = {_gasolinera} AND NumeroTanque = {tanque} ORDER BY Transaccion DESC";

                using var connection = new SqlConnection(_connectionString);
                var req = await connection.ExecuteQueryAsync<LecturaDTO>(sql);

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
    }
}
