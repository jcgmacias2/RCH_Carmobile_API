using ADDESAPI.Core.PreventaCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GTCQRS
{
    public interface IPreventaService
    {
        Task<Result> Add(AddPreventaDTO request);
        Task<ResultMultiple<Preventa>> GetPreventas(GetPreventasDTO request);
        Task<ResultSingle<Preventa>> GetPreventa(GetPreventaDTO request);
        Task<Result> SetStatus(SetPreventaStatusDTO request);
    }
}
