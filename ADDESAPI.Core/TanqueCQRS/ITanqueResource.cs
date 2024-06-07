using ADDESAPI.Core.EstacionCQRS.DTO;
using ADDESAPI.Core.TanqueCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.TanqueCQRS
{
    public interface ITanqueResource
    {
        Task<ResultMultiple<EstacionTanques>> GetTanques();
        Task<ResultSingle<LecturaDTO>> GetUltimaLectura(int tanque);
    }
}
