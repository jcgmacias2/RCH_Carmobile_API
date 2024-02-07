using ADDESAPI.Core.TipoCambioDTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.TipoCambioDTO
{
    public interface ITipoCambioResource
    {
        Task<ResultSingle<vTipoCambio>> GetTipoCambio(int estacion, string fecha);
    }
}
