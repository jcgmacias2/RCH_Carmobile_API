using ADDESAPI.Core.CorteCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.CorteCQRS
{
    public interface ICorteService
    {
        Task<ResultSingle<CorteDTO>> GetCorteColaborador(RequestCorteDTO req);
    }
}
