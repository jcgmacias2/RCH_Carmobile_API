using ADDESAPI.Core;
using ADDESAPI.Core.ImpresoraCQRS;
using ADDESAPI.Core.ImpresoraCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ImpresoraController : ControllerBase
    {
        private readonly ILogger<ImpresoraController> _logger;
        private readonly IImpresoraService _service;

        public ImpresoraController(ILogger<ImpresoraController> logger, IImpresoraService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ResultMultiple<Impresoras>> GetImpresoras()
        {
            return await _service.GetImpresoras();

        }
    }
}
