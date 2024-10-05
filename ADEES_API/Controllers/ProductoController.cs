using ADDESAPI.Core;
using ADDESAPI.Core.ProducoCQRS;
using ADDESAPI.Core.ProducoCQRS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADEES_API.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly ILogger<ProductoController> _logger;
        private readonly IProductoService _service;

        public ProductoController(ILogger<ProductoController> logger, IProductoService service)
        {
            _logger = logger;
            _service = service;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<ResultMultiple<FamiliaDTO>> GetFamilias()
        {
            return await _service.GetFamilias();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultMultiple<ProductoDTO>> GetProductosFamilia(ProductoReqDTO req)
        {
            return await _service.GetProductosFamilia(req); 
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<ProductoDTO>> GetProductoCB(ProductoCodigoReqDTO req)
        {
            return await _service.GetProductoCB(req);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<Result> SetProductoTicket(ProductoRequestDTO req)
        {
            return await _service.SetProductoTicket(req);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultSingle<int>> SetProducto(ProductoRequestDTO req)
        {
            return await _service.SetProducto(req);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ResultMultiple<ProductoDTO>> Buscar(FindProductReq req)
        {
            return await _service.Buscar(req);
        }
    }
}
