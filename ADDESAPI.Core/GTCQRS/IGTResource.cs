using ADDESAPI.Core.GTCQRS.DTO;
using ADDESAPI.Core.PresetCQRS.DTO;
using ADDESAPI.Core.ProducoCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GTCQRS
{
    public interface IGTResource
    {
        Task<ResultSingle<string>> GetToken();
        Task<ResultSingle<GTCommandResponse>> SendCommand(string command, string token);
        Task<Result> SetPreset(string token, PresetDTO preset/*, string jsonPreset*/);
    }
}
