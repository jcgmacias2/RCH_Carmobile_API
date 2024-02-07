using ADDESAPI.Core;
using ADDESAPI.Core.BambuCQRS;
using ADDESAPI.Core.BambuCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BambuController : ControllerBase
    {
        private readonly ILogger<BambuController> _logger;
        private readonly IBambuService _service;

        public BambuController(ILogger<BambuController> logger, IBambuService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<Result> FuelingQR(GenericRequest<RequestFuelingQrDTO> request)
        {
            return await _service.FuelingQR(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<QR>> GetQrPago(GenericRequest<RequestGetQrDTO> request)
        {
            return await _service.GetQrPago(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<QR>> SetQrPago(GenericRequest<RequestSetQrDTO> request)
        {
            return await _service.SetQrPago(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<Result> CancelQrPago(GenericRequest<RequestGetQrDTO> request)
        {
            return await _service.CancelQrPago(request.Data);
        }

    }
}
