using ADDESAPI.Core;
using ADDESAPI.Core.FajillaCQRS;
using ADDESAPI.Core.FajillaCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class FajillaController : ControllerBase
    {
        private readonly ILogger<FajillaController> _logger;
        private readonly IFajillaService _service;

        public FajillaController(ILogger<FajillaController> logger, IFajillaService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultMultiple<vFajillas>> GetFajillasColaborador(GenericRequest<ReqFajillas> req)
        {
            return await _service.GetFajillasColaborador(req.Data);
        }
    }
}
