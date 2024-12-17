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
        Task<Result> SetTypeBombas(SetBombasTypeDTO request);
        Task<Result> SetTypeBomba(SetBombaTypeDTO request);
        Task<ResultSingle<BombasTypeDTO>> GetTypeBombas();
        Task<ResultMultiple<EstructuraBombaDTO>> GetEstructuraBomba(GetEstructuraDTO request);
        Task<Result> CancelarPreset(GetEstructuraDTO request);
        Task<ResultSingle<ApiGtAnticipoRes>> AddAnticipo(AddAnticipoDTO req);
        
    }
}
