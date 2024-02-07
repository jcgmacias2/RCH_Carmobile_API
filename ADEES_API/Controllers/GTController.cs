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

    }
}
