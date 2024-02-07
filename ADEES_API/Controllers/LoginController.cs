using ADDESAPI.Core;
using ADDESAPI.Core.Colaborador;
using ADDESAPI.Core.Colaborador.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly IColaboradorService _service;

        public LoginController(ILogger<LoginController> logger, IColaboradorService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<ColaboradorDTO>> Login(GenericRequest<RequestLoginDTO> request)
        {
            var data = request.Data;
            return await _service.Login(data);

        }

    }
}
