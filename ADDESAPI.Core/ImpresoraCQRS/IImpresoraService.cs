using ADDESAPI.Core.ImpresoraCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ImpresoraCQRS
{
    public interface IImpresoraService
    {
        Task<ResultMultiple<Impresoras>> GetImpresoras();
    }
}
