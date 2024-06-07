using ADDESAPI.Core.DespachosCQRS.DTO;

namespace ADDESAPI.Core.DespachosCQRS
{
    public interface IDespachosService
    {
        //Task<ResultMultiple<DespachoDTO>> GetDespachos(RequestTransaccionesDTO request);
        Task<ResultMultiple<DespachoAppDTO>> GetDespachos(RequestTransaccionesDTO request);
        Task<ResultSingle<DespachoDTO>> GetDespachoByTransaccion(RequestTransaccionDTO requestDTO);
        Task<Result> SetTipoPago(ReqTransaccionTpDTO requestDTO);
        Task<ResultSingle<RedemptionDTO>> Redemption(RedemptionReq req);
        Task<ResultSingle<RedemptionDTO>> RewardRedemption(RedemptionReq req);
    }
}
