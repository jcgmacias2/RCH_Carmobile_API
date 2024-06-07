using ADDESAPI.Core;
using ADDESAPI.Core.VentaCQRS;
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
    public class VentaResource : IVentaResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly int _gasolinera;
        public VentaResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
        }
        public async Task<ResultMultiple<vVenta>> GetVentaTurno(string fecha, int turno, int bomba)
        {
            ResultMultiple<vVenta> Result = new ResultMultiple<vVenta>();
            try
            {
                string sql = $"SELECT Fecha, Gasolinera, Turno, Isla, Bomba, NoBomba, IdProducto, Producto, Cantidad, Total FROM vVenta WHERE Fecha = '{fecha}' AND Turno = {turno} AND NoBomba = {bomba}";
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.ExecuteQueryAsync<vVenta>(sql);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = "No se encontraron Ventas";
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
                Result.Error = "Error al obtener las Ventas";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultMultiple<vVenta>> GetVentaDespachosTurno(string fecha, int turno, int noEmpleado)
        {
            ResultMultiple<vVenta> Result = new ResultMultiple<vVenta>();
            try
            {
                string sql = $"SELECT Fecha, Gasolinera, Turno, Isla, Bomba, NoBomba, IdProducto, Producto, Cantidad, Total, NoEmpleado, TipoPago FROM vVentaDespachos WHERE Fecha = '{fecha}' AND Turno = {turno} AND NoEmpleado = {noEmpleado} AND IdProducto != 0";
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.ExecuteQueryAsync<vVenta>(sql);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = "No se encontraron Ventas";
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
                Result.Error = "Error al obtener las Ventas";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
