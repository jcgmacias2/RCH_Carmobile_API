using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GetnetCQRS
{
    public interface IGetnetResource
    {
        Task<ResultSingle<GetnetTransaccionesCorteDTO>> GetTransaccionesTurnoVendedor(string fecha, int estacion, int turno, int noEmpleado);
    }
}
