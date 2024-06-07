using ADDESAPI.Core;
using ADDESAPI.Core.PreventaCQRS;
using ADDESAPI.Core.PreventaCQRS.DTO;
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
    public class PreventaResource : IPreventaResource
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly int _gasolinera;

        public PreventaResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
        }

        public async Task<Result> Add(AddPreventaDTO request)
        {
            Result Result = new Result();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var param = new
                    {
                        Gasolinera = _gasolinera,
                        Tipo = request.Tipo,
                        DescripcionTipo = request.DescripcionTipo,
                        Moneda = request.Moneda,
                        TipoCambio = request.TipoCambio,
                        IdProducto = request.IdProducto,
                        Producto = request.Producto,
                        Bomba = request.Bomba,
                        TipoPago = request.TipoPago,
                        Cantidad = request.Cantidad,
                        Importe = request.Importe,
                        Usuario = request.Usuario,
                        Fecha = DateTime.ParseExact(request.Fecha, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture),
                        Turno = request.Turno,
                        FechaRegistro = DateTime.Now,
                        Estatus = 1,
                        FechaModificacion = DateTime.Now,
                    };

                    var result = connection.ExecuteScalar<int>("INSERT INTO Preventa OUTPUT INSERTED.ID VALUES (@Gasolinera, @Tipo, @DescripcionTipo, @Moneda, @TipoCambio, @IdProducto, @Producto, @Bomba, @TipoPago, @Cantidad, @Importe, @Usuario, @Fecha, @Turno, @FechaRegistro, @Estatus, @FechaModificacion);", param);

                    if (result > 0)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"Preventa guardada";
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"Error al guardar preventa";
                    }
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al guardar preventa";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<ResultMultiple<Preventa>> GetPreventas(DateTime fecha, int bomba)
        {
            ResultMultiple<Preventa> Result = new ResultMultiple<Preventa>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<Preventa>(r => r.Gasolinera == _gasolinera && r.Fecha == fecha && r.Bomba == bomba && r.Estatus == 1);

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
        public async Task<ResultSingle<Preventa>> GetPreventa(int id)
        {
            ResultSingle<Preventa> Result = new ResultSingle<Preventa>();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<Preventa>(r => r.Id == id);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = "No se encontro el registro";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Registro encontrado";
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
        public async Task<Result> SetStatus(SetPreventaStatusDTO request)
        {
            Result Result = new Result();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var param = new
                    {
                        Id = request.Id,
                        Estatus = request.Estatus
                    };

                    var result = connection.ExecuteNonQuery("UPDATE Preventa SET Estatus = @Estatus, FechaModificacion = GETDATE() WHERE Id = @Id", param);

                    if (result > 0)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"Preventa actualizada";
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"Error al actualizar preventa";
                    }
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al actualizar preventa";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
    }
}
