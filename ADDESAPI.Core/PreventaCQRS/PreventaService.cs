using ADDESAPI.Core.GTCQRS;
using ADDESAPI.Core.PreventaCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.PreventaCQRS
{
    public class PreventaService : IPreventaService
    {
        private readonly IPreventaResource _resource;

        public PreventaService(IPreventaResource resource)
        {
            _resource = resource;
        }

        public async Task<Result> Add(AddPreventaDTO request)
        {
            Result Result = new Result();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Tipo", Value = request.Tipo.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Moneda", Value = request.Moneda.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = request.Bomba.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "TipoPago", Value = request.TipoPago.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(double), ParameterName = "Cantidad", Value = request.Cantidad.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Usuario", Value = request.Usuario.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Fecha", Value = request.Fecha.ToString() });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Turno", Value = request.Turno.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultPreset = await _resource.Add(request);
                Result.Success = ResultPreset.Success;
                Result.Error = ResultPreset.Error;
                Result.Message = ResultPreset.Message;                

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error en Preventa";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<ResultMultiple<Preventa>> GetPreventas(GetPreventasDTO request)
        {
            ResultMultiple<Preventa> Result = new ResultMultiple<Preventa>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Fecha", Value = request.Fecha });
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Bomba", Value = request.Bomba.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }
                
                DateTime date = DateTime.ParseExact(request.Fecha, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

                Result = await _resource.GetPreventas(date, request.Bomba);               

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<ResultSingle<Preventa>> GetPreventa(GetPreventaDTO request)
        {
            ResultSingle<Preventa> Result = new ResultSingle<Preventa>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Id", Value = request.Id.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                Result = await _resource.GetPreventa(request.Id);

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
        public async Task<Result> SetStatus(SetPreventaStatusDTO request)
        {
            Result Result = new Result();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(int), ParameterName = "Id", Value = request.Id.ToString() });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                Result = await _resource.SetStatus(request);

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
    }
}
