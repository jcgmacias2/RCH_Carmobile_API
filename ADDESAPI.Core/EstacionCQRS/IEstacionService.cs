using ADDESAPI.Core.EstacionCQRS.DTO;
using ADDESAPI.Core.GTCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.EstacionCQRS
{
    public interface IEstacionService
    {
        Task<ResultMultiple<vBombas>> GetBombas();
        Task<ResultMultiple<EstacionCombustiblesDTO>> GetCombustibles();
        Task<ResultMultiple<PrecioBombaGtDTO>> GetPrecios(PrecioGtReqDTO req);
        Task<ResultSingle<vGasolinera>> GetGasolinera();
        Task<ResultSingle<TurnoActualDTO>> GetTurno();
    }
}
