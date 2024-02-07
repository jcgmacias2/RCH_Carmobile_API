using ADDESAPI.Core.FajillaCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.FajillaCQRS
{
    public class FajillaService : IFajillaService
    {
        private readonly IFajillaResource _resource;
        public FajillaService(IFajillaResource resource)
        {
            _resource = resource;
        }
        public async Task<ResultMultiple<DTO.vFajillas>> GetFajillasColaborador(ReqFajillas req)
        {
            ResultMultiple<vFajillas> Result = new ResultMultiple<vFajillas>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Fecha", Value = req.Fecha });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "NoEmpleado", Value = req.NoEmpleado.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Estacion", Value = req.Estacion.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }


                Result = await _resource.GetFajillasColaborador(req.Fecha, req.NoEmpleado, req.Estacion, req.Turno);
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
