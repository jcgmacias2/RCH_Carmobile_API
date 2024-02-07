using ADDESAPI.Core;
using ADDESAPI.Core.EstacionCQRS;
using ADDESAPI.Core.EstacionCQRS.DTO;
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
    public class EstacionResource : IEstacionResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly int _estacion;
        public readonly int _gasolinera;

        public EstacionResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
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
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<vGasolinera>(r => r.Codigo == _gasolinera);

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
    }
}
