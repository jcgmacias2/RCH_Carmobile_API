using ADDESAPI.Core.Asignacion.DTO;
using ADDESAPI.Core.AsignacionCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.AsignacionCQRS
{
    public interface IAsignacionService
    {
        Task<ResultSingle<AsignacionesDTO>> GetAsignaciones(GetAsignacionesReqDTO req);
        Task<ResultSingle<AsignacionColaboradorTurno>> GetAsignacion(GetAsignacionReqDTO req);
    }
}
