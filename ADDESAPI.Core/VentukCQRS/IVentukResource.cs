using ADDESAPI.Core.VentukCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.VentukCQRS
{
    public interface IVentukResource 
    {
        Task<ResultSingle<Asistencia>> GetAsistencia(int idEmpleado, DateTime fecha);
    }
}
