using ADDESAPI.Core;
using ADDESAPI.Core.GTCQRS.DTO;
using ADDESAPI.Core.PresetCQRS;
using ADDESAPI.Core.PresetCQRS.DTO;
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
        public async Task<Result> SavePreset(PresetDTO request)
        {
            Result Result = new Result();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var param = new { @Gasolinera = _gasolinera,
                        NoEmpleado = request.NoEmpleado,
                        NoBomba = request.Bomba,
                        Tipo = request.UMedida,
                        Cantidad = request.Cantidad,
                        Grado = request.Grado,
                        DespachoAnterior = 0,
                        TipoPago = request.TipoPago,
                        QR = "",// request.QrPago,
                        RFC = request.RFC,
                        Total = request.Total,
                        Descuento = request.Descuento,
                        TotalConDescuento = request.Total - request.Descuento,
                        QrCupon = "",// request.QrCupon,
                        Cupon = "",//request.Cupon
                    };
                    var affectedRows = connection.ExecuteNonQuery("EXEC [dbo].[SP_PRESET_GATEWAY] @Gasolinera, @NoEmpleado, @NoBomba, @Tipo, @Cantidad, @Grado, @DespachoAnterior, @TipoPago, @QR, @RFC, @Total, @Descuento, @TotalConDescuento, @QrCupon, @Cupon;", param);
                    if (affectedRows > 0)
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
    }
}
