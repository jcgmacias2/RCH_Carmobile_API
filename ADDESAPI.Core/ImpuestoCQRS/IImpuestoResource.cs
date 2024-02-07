using ADDESAPI.Core.ImpuestoCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ImpuestoCQRS
{
    public interface IImpuestoResource
    {
        Task<ResultSingle<vImpuesto>> GetImpuestoProducto(int fecha, int producto);
        Task<ResultSingle<vImpuesto>> GetImpuestoProducto(int producto);
    }
}
