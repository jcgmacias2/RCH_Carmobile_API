using Proteo5.HL;

namespace ADDESAPI.Infrastructure
{
   
    
    public static class ResultData<T>
    {
        public static Result<List<T>> CheckData(IEnumerable<T> dataSet)
        {
            try
            {
                if (dataSet.Any())
                {
                    return new Result<List<T>>(ResultsStates.success) { Data = dataSet.ToList() };
                }
                else
                {
                    return new Result<List<T>>(ResultsStates.empty);
                }
            }
            catch (Exception ex)
            {
                return new Result<List<T>>(ResultsStates.error) { Message = $"Error {ex.Message}" };
            }
        }

        public static Result<T> CheckDataSingle(IEnumerable<T> dataSet)
        {
            try
            {
                if (dataSet.Any())
                {
                    return new Result<T>(ResultsStates.success) { Data = dataSet.FirstOrDefault() };
                }
                else
                {
                    return new Result<T>(ResultsStates.empty);
                }
            }
            catch (Exception ex)
            {
                return new Result<T>(ResultsStates.error) { Message = $"Error {ex.Message}" };
            }
        }

        internal static object Multiple(IEnumerable<object> dataSet)
        {
            throw new NotImplementedException();
        }
    }
    
   
}
