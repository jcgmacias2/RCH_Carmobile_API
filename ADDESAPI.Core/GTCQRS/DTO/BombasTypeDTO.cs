using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GTCQRS.DTO
{
    public class SetBombasTypeDTO
    {
        public string Usuario { get; set; }
        public List<int> fullService { get; set; }
        public List<int> selfService { get; set; }
    }
    public class SetBombaTypeDTO
    {
        public string Usuario { get; set; }
        public int Bomba { get; set; }
        public int Modo { get; set; }
    }
    public class BombasTypeDTO
    {
        public List<int> fullService { get; set; }
        public List<int> selfService { get; set; }
    }
    public class GetEstructuraDTO
    {
        public int Bomba { get; set; }
    }
}
