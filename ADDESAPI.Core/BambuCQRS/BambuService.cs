using ADDESAPI.Core.BambuCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.BambuCQRS
{
    public class BambuService : IBambuService
    {
        private readonly IBambuResource _resource;

        public BambuService(IBambuResource bambuResource)
        {
            _resource = bambuResource;
        }
        public async Task<Result> FuelingQR(RequestFuelingQrDTO request)
        {
            Result Result = new Result();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Usuario(user_id)", Value = request.user_id });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "ticket", Value = request.ticket });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Cantidad(charge)", Value = request.charge });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Combustible(fuel_type)", Value = request.fuel_type });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Fecha(date)", Value = request.date });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data.token;

                Result = await _resource.FuelingQR(request, token);

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Excepcion";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<QR>> GetQrPago(RequestGetQrDTO request)
        {
            ResultSingle<QR> Result = new ResultSingle<QR>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "QR", Value = request.QR});

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data.token;

                Result = await _resource.GetQrPago(request, token);



            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Excepcion";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<ResultSingle<QR>> SetQrPago(RequestSetQrDTO request)
        {
            ResultSingle<QR> Result = new ResultSingle<QR>();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "QR", Value = request.QR });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Estacion", Value = request.Estacion });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "Bomba", Value = request.Bomba });
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "NoEmpleado", Value = request.NoEmpleado });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data.token;

                Result = await _resource.SetQrPago(request, token);



            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Excepcion";
                Result.Message = ex.Message;
            }
            return Result;
        }
        public async Task<Result> CancelQrPago(RequestGetQrDTO request)
        {
            Result Result = new Result();
            try
            {
                Validator validator = new Validator();
                List<Validate> valuesToValidate = new List<Validate>();
                valuesToValidate.Add(new Validate { DataType = typeof(string), ParameterName = "QR", Value = request.QR });

                Result ResultValidate = validator.GetValidate(valuesToValidate);
                if (!ResultValidate.Success)
                {
                    Result.Success = ResultValidate.Success;
                    Result.Error = ResultValidate.Error;
                    Result.Message = ResultValidate.Message;
                    return Result;
                }

                var ResultToken = await _resource.GetToken();
                if (!ResultToken.Success)
                {
                    Result.Success = ResultToken.Success;
                    Result.Error = ResultToken.Error;
                    Result.Message = ResultToken.Message;
                    return Result;
                }
                string token = ResultToken.Data.token;

                Result = await _resource.CancelQrPago(request, token);
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Excepcion";
                Result.Message = ex.Message;
            }
            return Result;
        }
    }
}
