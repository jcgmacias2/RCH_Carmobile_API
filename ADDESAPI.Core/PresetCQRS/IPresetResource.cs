using ADDESAPI.Core.PresetCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.PresetCQRS
{
    public interface IPresetResource
    {
        Task<Result> SavePreset(Preset request);
        Task<Result> PresetGatewayRes(PresetGatewayRes request);
        Task<ResultMultiple<Preset>> GetPresets(int pageSize, int page, int bomba, int noEmpleado);
    }
}
