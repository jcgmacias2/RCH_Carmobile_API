using ADDESAPI.Core;
using ADDESAPI.Core.EstacionCQRS;
using ADDESAPI.Core.EstacionCQRS.DTO;
using ADDESAPI.Core.GTCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EstacionController : ControllerBase
    {
        private readonly ILogger<EstacionController> _logger;
        private readonly IEstacionService _service;

        public EstacionController(ILogger<EstacionController> logger, IEstacionService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ResultMultiple<vBombas>> GetBombas()
        {
            return await _service.GetBombas();
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ResultMultiple<EstacionCombustiblesDTO>> GetCombustibles()
        {
            return await _service.GetCombustibles();
        }
        [HttpPost]
        [Route("[action]")] 
        public async Task<ResultMultiple<PrecioBombaGtDTO>> GetPrecios(GenericRequest<PrecioGtReqDTO> req)
        {
            return await _service.GetPrecios(req.Data);
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ResultSingle<vGasolinera>> GetGasolinera()
        {
            return await _service.GetGasolinera(); 
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ResultSingle<TurnoActualDTO>> GetTurno()
        {
            return await _service.GetTurno();
        }
    }
}
