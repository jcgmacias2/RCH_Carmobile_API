using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core
{
    public class GenericRequest<T>
    {
        public T Data { get; set; }
    }
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
    }
    public class ResultMultiple<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public IList<T> Data { get; set; }
    }
    public class ResultSingle<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public T Data { get; set; }
    }
    public class ResultVentuk<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public IList<T> Data = new List<T>();
    }
    public class ResultBambu<T>
    {
        public T data { get; set; }
    }
    public class ResultADDES<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public T response { get; set; }
    }
}
