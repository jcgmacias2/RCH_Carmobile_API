using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core
{
    public class Validator
    {
        public Validator()
        {
            
        }
        public Result GetValidate(List<Validate> values)
        {
            Result Result = new Result();
            Result.Success = true;
            try
            {
                Result validate = new Result();
                foreach (var value in values)
                {
                    if (value.DataType == typeof(int))
                    {
                        validate = ValidateInt(value);
                        if (!validate.Success)
                        {
                            Result.Success = false;
                            Result.Message += $"{validate.Message}. ";
                            Result.Error = "Error";
                        }
                    }
                    else if (value.DataType == typeof(double))
                    {
                        validate = ValidateDouble(value);
                        if (!validate.Success)
                        {
                            Result.Success = false;
                            Result.Message += $"{validate.Message}. ";
                            Result.Error = "Error";
                        }
                    }
                    else if (value.DataType == typeof(string))
                    {
                        validate = ValidateString(value);
                        if (!validate.Success)
                        {
                            Result.Success = false;
                            Result.Message += $"{validate.Message}. ";
                            Result.Error = "Error";                        
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error al validar datos de entrada. {ex.Message}";
            }
            return Result;
        }
        public Result ValidateInt(Validate value)
        {
            Result result = new Result();
            try
            {
                if (value.Value == "")
                {
                    result.Success = false;
                    result.Error = "Error ";
                    result.Message = $"El parametro {value.ParameterName} no puede ser vacio";
                    return result;
                }

                int.Parse(value.Value);
                result.Success = true;
                result.Error = "";
                result.Message = "";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = $"Error en el parametro{value.ParameterName}";
                result.Message = $"{ex.Message}";
            }
            return result;
        }
        public Result ValidateDouble(Validate value)
        {
            Result result = new Result();
            try
            {
                if (value.Value == "")
                {
                    result.Success = false;
                    result.Error = "Error ";
                    result.Message = $"El parametro {value.ParameterName} no puede ser vacio";
                    return result;
                }

                double.Parse(value.Value);
                result.Success = true;
                result.Error = "";
                result.Message = "";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = $"Error en el parametro{value.ParameterName}";
                result.Message = $"{ex.Message}";
            }
            return result;
        }
        public Result ValidateString(Validate value)
        {
            Result result = new Result();
            try
            {
                if (value.Value == "")
                {
                    result.Success = false;
                    result.Error = "Error ";
                    result.Message = $"El parametro {value.ParameterName} no puede ser vacio";
                    return result;
                }

                result.Success = true;
                result.Error = "";
                result.Message = "";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = $"Error en el parametro{value.ParameterName}";
                result.Message = $"{ex.Message}";
            }
            return result;
        }
    }

    public class Validates
    {
        public bool Success { get; set; }
        public List<string> Error { get; set; }
        public string Message { get; set; }
    }
    public class Validate
    {
        public Type DataType { get; set; }
        public string Value { get; set; }
        public string ParameterName { get; set; }
    }

    
}
