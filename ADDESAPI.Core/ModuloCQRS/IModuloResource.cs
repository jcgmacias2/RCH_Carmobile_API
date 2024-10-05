using ADDESAPI.Core.ModuloCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ModuloCQRS
{
    public interface IModuloResource
    {
        Task<ResultMultiple<ModuloDTO>> GetModulos();
    }
}
