using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.CorteCQRS
{
    public interface ICorteResource
    {
        Task<Result> GetCorte(string fecha, int turno, int bomba);
    }
}
