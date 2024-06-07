using ADDESAPI.Core.ProducoCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ProducoCQRS
{
    public interface IProductoResource
    {
        Task<ResultMultiple<FamiliaGT>> GetFamilias(string token);
        Task<ResultMultiple<ProductoGT>> GetProductosFamilia(string token, int familia);
        Task<ResultSingle<TicketGT>> SetProductoTicket(string token, int bomba, string jsonProductos, int noEmpleado);
        Task<ResultSingle<TicketGT>> SetProducto(string token, int bomba, string jsonProductos, int noEmpleado);
        Task<ResultSingle<vProductos>> GetProductoCB(string token, string codigo);
    }
}
