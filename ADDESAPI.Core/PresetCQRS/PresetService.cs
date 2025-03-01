using ADDESAPI.Core.Colaborador;
using ADDESAPI.Core.PresetCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.PresetCQRS
{
    public class PresetService : IPresetService
    {
        private readonly IPresetResource _resource;
        private readonly IColaboradorResource _resourceColaborador;

        public PresetService(IPresetResource resource, IColaboradorResource resourceColaborador)
        {
            _resource = resource;
            _resourceColaborador = resourceColaborador;

        }

        public async Task<ResultMultiple<Preset>> GetPresets(GetPresetsDTO req, string user)
        {
            ResultMultiple < Preset > Result = new ResultMultiple<Preset>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "PageSize", Value = req.PageSize.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Page", Value = req.Page.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = req.Bomba.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {

                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultC = await _resourceColaborador.GetColaborador(user);
                if (!ResultC.Success) 
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }
                var Colaborador = ResultC.Data;

                Result = await _resource.GetPresets(req.PageSize, req.Page, req.Bomba, Colaborador.NumeroVentuk);
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al consultar los presets";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
    }
}
