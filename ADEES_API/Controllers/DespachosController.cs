using ADDESAPI.Core;
using ADDESAPI.Core.DespachosCQRS;
using ADDESAPI.Core.DespachosCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DespachosController : ControllerBase
    {
        private readonly ILogger<DespachosController> _logger;
        private readonly IDespachosService _service;

        public DespachosController(ILogger<DespachosController> logger, IDespachosService service)
        { 
            _logger = logger;
            _service = service;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultMultiple<DespachoAppDTO>> GetDespachos(GenericRequest<RequestTransaccionesDTO> request)
        {
            return await _service.GetDespachos(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<DespachoDTO>> GetDespacho(GenericRequest<RequestTransaccionDTO> request)
        {
            return await _service.GetDespachoByTransaccion(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<Result> SetTipoPago(GenericRequest<ReqTransaccionTpDTO> request)
        {
            return await _service.SetTipoPago(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<RedemptionDTO>> Redemption(GenericRequest<RedemptionReq> request)
        {
            return await _service.Redemption(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<RedemptionDTO>> RewardRedemption(GenericRequest<RedemptionReq> request)
        {
            return await _service.RewardRedemption(request.Data);
        }
    }
}
