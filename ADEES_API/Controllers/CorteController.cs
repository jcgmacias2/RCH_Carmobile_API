using ADDESAPI.Core;
using ADDESAPI.Core.CorteCQRS;
using ADDESAPI.Core.CorteCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[Authorize]
    public class CorteController : ControllerBase
    {
        private readonly ILogger<CorteController> _logger;
        private readonly ICorteService _service;

        public CorteController(ILogger<CorteController> logger, ICorteService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<CorteDTO>> GetCorteColaborador(GenericRequest<RequestCorteDTO> req)
        {
            return await _service.GetCorteColaborador(req.Data);
        }
    }
}
