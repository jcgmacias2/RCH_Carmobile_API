using ADDESAPI.Core.GTCQRS.DTO;
using ADDESAPI.Core.PresetCQRS.DTO;
using ADDESAPI.Core.ProducoCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GTCQRS
{
    public interface IGTResource
    {
        Task<ResultSingle<string>> GetToken();
        Task<ResultSingle<GTCommandResponse>> SendCommand(string command, string token);
        Task<Result> SetPreset(string token, PresetDTO preset/*, string jsonPreset*/);
        Task<Result> SetTypeBombas(string token, string json);
        Task<ResultMultiple<ApiGtBombasResponse>> GetTypeBombas(string token);
        Task<Result> SetTypeBomba(string token, int bomba, int modo);
        Task<Result> RestartSGPM(string token);
        Task<ResultMultiple<EstructuraBombaDTO>> GetEstructuraBomba(string token, int bomba);
        Task<Result> CancelarPreset(string token, int bomba);
        Task<ResultSingle<ApiGtAnticipoRes>> AddAnticipo(string token, string json);
        Task<Result> SetDescuento(int transaccion, double descuento, string cardNumber, string PromoDesc, int promoCode, double litrosRedimidos, string nombreCliente, int noEmpleado, string vendedor);
        Task<ResultSingle<DescuentoDTO>> GetDescuento(string transaccion);
        //Task<Result> SetDiscount(int transaccion, double discount, string cardNumber, string promotion, int idPromotion, double littersApp, string clientName);
        //Task<ResultSingle<DiscountDTO>> GetDiscount(string transaccion);
    }
}
