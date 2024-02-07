using ADDESAPI.Core;
using ADDESAPI.Core.FajillaCQRS;
using ADDESAPI.Core.FajillaCQRS.DTO;
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
    public class FajillaResource : IFajillaResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly string _connectionStringAddes; 
        public readonly int _gasolinera;
        public FajillaResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _connectionStringAddes = _configuration["ConnectionStrings:AddesConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
        }
        public async Task<ResultMultiple<vFajillas>> GetFajillasColaborador(string fecha, int noEmpleado, int estacion, int turno)
        {
            ResultMultiple<vFajillas> Result = new ResultMultiple<vFajillas>();
            try
            {
                vFajillas Fajillas = new vFajillas();

                string sql = $"SELECT * FROM vFajillas WHERE CAST(Fecha AS DATE) = '{fecha}' AND NoEmpleado = {noEmpleado} AND NoEstacion = {estacion} AND Turno = {turno}";
                using var connection = new SqlConnection(_connectionStringAddes);
                var request = await connection.ExecuteQueryAsync<vFajillas>(sql);

                if (request == null || request.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = "No se encotraron fajillas";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "";
                    Result.Data = request.ToList();
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al obtener las fajillas";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
