using ADDESAPI.Core;
using ADDESAPI.Core.TanqueCQRS;
using ADDESAPI.Core.TanqueCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TanqueController : ControllerBase
    {
        private readonly ILogger<TanqueController> _logger;
        private readonly ITanqueService _service;

        public TanqueController(ILogger<TanqueController> logger, ITanqueService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ResultSingle<LecturasDTO>> GetLecturas()
        {
            return await _service.GetLecturas();
        }
    }
}
