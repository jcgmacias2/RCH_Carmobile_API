using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.PresetCQRS.DTO
{
    public class GetPresetsDTO
    {
        public int Bomba { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
