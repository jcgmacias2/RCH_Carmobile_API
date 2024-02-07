using ADDESAPI.Core.Colaborador.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.Colaborador
{
    public interface IColaboradorService
    {
        Task<ResultSingle<ColaboradorDTO>> Login(RequestLoginDTO request);
    }
}
