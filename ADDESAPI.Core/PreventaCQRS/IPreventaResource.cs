using ADDESAPI.Core.PreventaCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.PreventaCQRS
{
    public interface IPreventaResource
    {
        Task<Result> Add(AddPreventaDTO request);
        Task<ResultMultiple<Preventa>> GetPreventas(DateTime fecha, int bomba);
        Task<ResultSingle<Preventa>> GetPreventa(int id);
        Task<Result> SetStatus(SetPreventaStatusDTO request);
    }
}
