using ADDESAPI.Core;
using ADDESAPI.Core.Asignacion;
using ADDESAPI.Core.Asignacion.DTO;
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
    public class AsignacionResource : IAsignacionResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;

        public AsignacionResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
        }
        public async Task<ResultMultiple<vAsignacion>> GetAsignacion(int noEmpleado, DateTime fecha)
        {
            ResultMultiple<vAsignacion> Result = new ResultMultiple<vAsignacion>();
            try
            {
                vAsignacion Asignacion = new vAsignacion();
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<vAsignacion>(r => r.NumeroEmpleado == noEmpleado && r.Fecha == fecha);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = "No se encotro asignación de bombas";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Asignacion encontrada";
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
