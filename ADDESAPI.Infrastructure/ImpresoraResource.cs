using ADDESAPI.Core;
using ADDESAPI.Core.ImpresoraCQRS;
using ADDESAPI.Core.ImpresoraCQRS.DTO;
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
    public class ImpresoraResource : IImpresoraResource
    {
        private readonly IConfiguration _configuration;
        private readonly int _estacion;
        private readonly int _gasolinera;
        public readonly string _connectionString;

        public ImpresoraResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        }
        public async Task<ResultMultiple<Impresoras>> GetImpresoras()
        {
            ResultMultiple<Impresoras> Result = new ResultMultiple<Impresoras>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<Impresoras>(r => r.Estatus == 1);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = "No se encontraron impresoras";
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
                Result.Error = "Error";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
