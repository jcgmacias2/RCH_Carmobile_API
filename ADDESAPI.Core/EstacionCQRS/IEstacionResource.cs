using ADDESAPI.Core.EstacionCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.EstacionCQRS
{
    public interface IEstacionResource
    {
        Task<ResultMultiple<vBombas>> GetBombas();
        Task<ResultMultiple<EstacionTanques>> GetTanques();
        Task<ResultSingle<vGasolinera>> GetGasolinera();
    }
}
