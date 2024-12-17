using ADDESAPI.Core;
using ADDESAPI.Core.DespachosCQRS.DTO;
using ADDESAPI.Core.GTCQRS.DTO;
using ADDESAPI.Core.PresetCQRS;
using ADDESAPI.Core.PresetCQRS.DTO;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ADDESAPI.Infrastructure
{
    public class PresetResource : IPresetResource
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly int _gasolinera;

        public PresetResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
        }
        public async Task<Result> SavePreset(Preset req)
        {
            Result Result = new Result();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var param = new { 
                        Gasolinera = _gasolinera,
                        Bomba = req.Bomba,
                        UMedida = req.UMedida,
                        Grado = req.Grado,
                        Cantidad = req.Cantidad,
                        Total = req.Total,
                        NoEmpleado = req.NoEmpleado,
                        Vendedor = req.Vendedor,
                        IdTipoPago = req.IdTipoPago,
                        Moneda = req.Moneda,
                        Estatus = req.Estatus,
                        Error = req.Error,
                        CardNumber = req.CardNumber,
                        LitrosRedimir = req.LitrosRedimir,
                        Descuento = req.Descuento
                    };
                    string sql = @"INSERT INTO Preset( Gasolinera,  Bomba,  UMedida,  Grado,  Cantidad,  Total,  NoEmpleado,  Vendedor,  IdTipoPago,  Moneda,  Estatus,  Error,  CardNumber,  LitrosRedimir,  Descuento, Fecha)
                                   VALUES			 (@Gasolinera, @Bomba, @UMedida, @Grado, @Cantidad, @Total, @NoEmpleado, @Vendedor, @IdTipoPago, @Moneda, @Estatus, @Error, @CardNumber, @LitrosRedimir, @Descuento, GETDATE())";
                    var affectedRecords = connection.ExecuteNonQuery(sql, param);
                    if (affectedRecords == 0)
                    {
                        Result.Success = false;
                        Result.Error = "Error al guardar Preset";
                        Result.Message = "No se logro insertar el registro en la tabla Preset de ADDES local";
                    }
                    else
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = "Registro guardado";
                    }
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al guardar el preset en addes local";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        //public async Task<Result> SavePreset(PresetDTO request)
        //{
        //    Result Result = new Result();
        //    try
        //    {
        //        using (var connection = new SqlConnection(_connectionString))
        //        {
        //            var param = new
        //            {
        //                @Gasolinera = _gasolinera,
        //                NoEmpleado = request.NoEmpleado,
        //                NoBomba = request.Bomba,
        //                Tipo = request.UMedida,
        //                Cantidad = request.Cantidad,
        //                Grado = request.Grado,
        //                DespachoAnterior = 0,
        //                TipoPago = request.TipoPago,
        //                QR = "",// request.QrPago,
        //                RFC = request.RFC,
        //                Total = request.Total,
        //                Descuento = request.Descuento,
        //                TotalConDescuento = request.Total - request.Descuento,
        //                QrCupon = "",// request.QrCupon,
        //                Cupon = "",//request.Cupon
        //            };
        //            var affectedRows = connection.ExecuteNonQuery("EXEC [dbo].[SP_PRESET_GATEWAY] @Gasolinera, @NoEmpleado, @NoBomba, @Tipo, @Cantidad, @Grado, @DespachoAnterior, @TipoPago, @QR, @RFC, @Total, @Descuento, @TotalConDescuento, @QrCupon, @Cupon;", param);
        //            if (affectedRows > 0)
        //            {
        //                Result.Success = true;
        //                Result.Error = "";
        //                Result.Message = $"Preset guardado";
        //            }
        //            else
        //            {
        //                Result.Success = false;
        //                Result.Error = "Error";
        //                Result.Message = $"Error al guardar el preset";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Result.Success = false;
        //        Result.Error = "Error al guardar el preset";
        //        Result.Message = $"{ex.Message}";
        //    }
        //    return Result;
        //}
        public async Task<Result> PresetGatewayRes(PresetGatewayRes request)
        {
            Result Result = new Result();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {

                    var id = connection.InsertAsync(PresetGatewayRes);
                    if (id != null)
                    {
                        Result.Success = true;
                        Result.Error = "";
                        Result.Message = $"Preset guardado";
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"Error al guardar el preset";
                    }
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al guardar el preset";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<ResultMultiple<Preset>> GetPresets(int pageSize, int page, int bomba)
        {
            ResultMultiple<Preset> Result = new ResultMultiple<Preset> ();
            try
            {
                string sql = @$"
                    DECLARE @PageSize	INT = {pageSize},
		                    @Page		INT = {page},
		                    @Max		INT,
		                    @Current	INT
                    SELECT @Max = MAX(Id) FROM Preset
                    SELECT @Current = @Max - (@PageSize * @Page)
                    SELECT TOP {pageSize} * FROM Preset WHERE Id <= @Current AND Bomba = {bomba} ORDER BY Id DESC";

                using (var connection = new SqlConnection(_connectionString))
                {
                    var req = await connection.ExecuteQueryAsync<Preset>(sql);

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
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al consultar los presets";
                Result.Message = $"{ex.Message}";
            }
            return Result;

        }
    }
}
