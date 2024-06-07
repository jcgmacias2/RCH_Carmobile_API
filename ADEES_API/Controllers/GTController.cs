using ADDESAPI.Core;
using ADDESAPI.Core.GTCQRS;
using ADDESAPI.Core.GTCQRS.DTO;
using ADDESAPI.Core.PresetCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GTController : ControllerBase
    {
        private readonly ILogger<GTController> _logger;
        private readonly IGTService _service;

        public GTController(ILogger<GTController> logger, IGTService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<Result> Preset(GenericRequest<PresetDTO> request)
        {
            return await _service.Preset(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<Result> SetTypeBombas(GenericRequest<SetBombasTypeDTO> request)
        {
            return await _service.SetTypeBombas(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<Result> SetTypeBomba(GenericRequest<SetBombaTypeDTO> request)
        {
            return await _service.SetTypeBomba(request.Data);
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ResultSingle<BombasTypeDTO>> GetTypeBombas()
        {
            return await _service.GetTypeBombas();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultMultiple<EstructuraBombaDTO>> GetEstructuraBomba(GenericRequest<GetEstructuraDTO> request)
        {
            return await _service.GetEstructuraBomba(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<Result> CancelarPreset(GenericRequest<GetEstructuraDTO> request)
        {
            return await _service.CancelarPreset(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<ApiGtAnticipoRes>> AddAnticipo(GenericRequest<AddAnticipoDTO> request)
        {
            return await _service.AddAnticipo(request.Data);
        }
    }
}
