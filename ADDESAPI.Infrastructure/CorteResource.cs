using ADDESAPI.Core;
using ADDESAPI.Core.CorteCQRS;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Infrastructure
{
    public class CorteResource : ICorteResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly int _gasolinera;
        public CorteResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
        }
        public async Task<Result> GetCorte(string fecha, int turno, int bomba)
        {
            Result Result = new Result();
            try
            {

            }
            catch (Exception ex)
            {

            }
            return Result;
        }
    }
}
