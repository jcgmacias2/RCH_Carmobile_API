using ADDESAPI.Core.ProducoCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ProducoCQRS
{
    public interface IProductoService
    {
        Task<ResultMultiple<FamiliaDTO>> GetFamilias();
        Task<ResultMultiple<ProductoDTO>> GetProductosFamilia(ProductoReqDTO req);
        Task<Result> SetProductoTicket(ProductoRequestDTO req);
        Task<ResultSingle<int>> SetProducto(ProductoRequestDTO req);
        Task<ResultSingle<ProductoDTO>> GetProductoCB(ProductoCodigoReqDTO req);
    }
}
