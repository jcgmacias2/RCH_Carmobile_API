using ADDESAPI.Core;
using ADDESAPI.Core.ModuloCQRS;
using ADDESAPI.Core.ModuloCQRS.DTO;
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
    public class ModuloResource : IModuloResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;

        public ModuloResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        }
        public async Task<ResultMultiple<ModuloDTO>> GetModulos()
        {
            ResultMultiple<ModuloDTO> Result = new ResultMultiple<ModuloDTO>();
            try
            {
                string sql = $"SELECT * FROM ModulosApp b(NOLOCK)";

                using var connection = new SqlConnection(_connectionString);
                var req = await connection.ExecuteQueryAsync<ModuloDTO>(sql);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = "No se encotraron registros";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Modulos encontrados";
                    Result.Data = req.ToList();
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
