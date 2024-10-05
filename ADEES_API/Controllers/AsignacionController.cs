using ADDESAPI.Core;
using ADDESAPI.Core.Asignacion.DTO;
using ADDESAPI.Core.AsignacionCQRS;
using ADDESAPI.Core.AsignacionCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AsignacionController : ControllerBase
    {
        private readonly ILogger<AsignacionController> _logger;
        private readonly IAsignacionService _service;

        public AsignacionController(ILogger<AsignacionController> logger, IAsignacionService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<AsignacionesDTO>> GetAsignaciones(GenericRequest<GetAsignacionesReqDTO> request)
        {
            return await _service.GetAsignaciones(request.Data);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<AsignacionColaboradorTurno>> GetAsignacion(GenericRequest<GetAsignacionReqDTO> request)
        {
            return await _service.GetAsignacion(request.Data);
        }
    }
}
