using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.VentaCQRS
{
    public interface IVentaResource
    {
        Task<ResultMultiple<vVenta>> GetVentaTurno(string fecha, int turno, int bomba);
        Task<ResultMultiple<vVenta>> GetVentaDespachosTurno(string fecha, int turno, int noEmpleado);
    }
}
