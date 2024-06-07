using ADDESAPI.Core.DespachosCQRS.DTO;

namespace ADDESAPI.Core.DespachosCQRS
{
    public interface IDespachosResource
    {
        //Task<ResultMultiple<vDespachos>> GetDespachos(int bomba);
        Task<ResultMultiple<DespachoAppDTO>> GetDespachos(int bomba);
        Task<ResultSingle<vDespachos>> GetDespacho(int despacho);
        Task<ResultMultiple<vDespachos>> GetDespachoDetalle(int despacho);
        Task<Result> SetTipoPago(int despacho, int tipoPago);
        //Task<ResultMultiple<DespachoDetalleAppDTO>> GetDespachosApp(string despachos);
        Task<ResultSingle<RedemptionDTO>> Redemption(RedemptionReq req);
        Task<ResultSingle<RedemptionDTO>> RewardRedemption(RedemptionReq req);
    }
}
