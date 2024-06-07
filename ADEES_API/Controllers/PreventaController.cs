using ADDESAPI.Core;
using ADDESAPI.Core.GTCQRS;
using ADDESAPI.Core.PreventaCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class PreventaController : ControllerBase
    {
        private readonly ILogger<PreventaController> _logger;
        private readonly IPreventaService _service;

        public PreventaController(ILogger<PreventaController> logger, IPreventaService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<Result> Add(AddPreventaDTO request)
        {
            return await _service.Add(request);

        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ResultMultiple<Preventa>> GetPreventas(GenericRequest<GetPreventasDTO> request)
        {
            return await _service.GetPreventas(request.Data);

        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<Preventa>> GetPreventa(GenericRequest<GetPreventaDTO> request)
        {
            return await _service.GetPreventa(request.Data);

        }
        [HttpPost]
        [Route("[action]")]
        public async Task<Result> SetStatus(GenericRequest<SetPreventaStatusDTO> request)
        {
            return await _service.SetStatus(request.Data);

        }
    }
}
