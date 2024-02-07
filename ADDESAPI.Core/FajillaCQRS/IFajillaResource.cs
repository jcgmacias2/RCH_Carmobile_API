using ADDESAPI.Core.FajillaCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.FajillaCQRS
{
    public interface IFajillaResource
    {
        Task<ResultMultiple<vFajillas>> GetFajillasColaborador(string fecha, int noEmpleado, int estacion, int turno);
    }
}
