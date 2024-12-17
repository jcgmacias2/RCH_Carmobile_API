using ADDESAPI.Core;
using ADDESAPI.Core.GTCQRS;
using ADDESAPI.Core.PresetCQRS;
using ADDESAPI.Core.PresetCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PresetController : Controller
    {
        private readonly ILogger<PresetController> _logger;
        private readonly IPresetService _service;
        public PresetController(ILogger<PresetController> logger, IPresetService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultMultiple<Preset>> GetPresets(GetPresetsDTO req)
        {
            return await _service.GetPresets(req);
        }
    }
}
