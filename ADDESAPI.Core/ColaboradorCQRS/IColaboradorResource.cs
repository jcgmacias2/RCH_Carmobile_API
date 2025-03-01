using ADDESAPI.Core.Colaborador.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.Colaborador
{
    public interface IColaboradorResource
    {
        Task<ResultSingle<vColaborador>> Login(string usuario, string password);
        Task<ResultSingle<string>> GenerateTokenJwt(string username);
        Task<ResultMultiple<ColaboradoresDTO>> GetColaboradores();
        Task<ResultSingle<ColaboradoresDTO>> GetColaborador(string user);
    }
}
