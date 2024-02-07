using ADDESAPI.Core.GTCQRS.DTO;
using ADDESAPI.Core.PresetCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GTCQRS
{
    public interface IGTService
    {
        Task<Result> Preset(PresetDTO request);
    }
}
