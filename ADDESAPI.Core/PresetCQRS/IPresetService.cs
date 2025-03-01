using ADDESAPI.Core.PresetCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.PresetCQRS
{
    public interface IPresetService
    {
        Task<ResultMultiple<Preset>> GetPresets(GetPresetsDTO req, string user);
    }
}
