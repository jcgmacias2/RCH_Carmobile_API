using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ImpuestoCQRS.DTO
{
    public class vImpuesto
    {
        public int Gasolinera { get; set; }
        public int FechaCG { get; set; }
        public int Producto { get; set; }
        public double TasaIVA { get; set; }
        public double CuotaIEPS { get; set; }
        public double Precio { get; set; }
    }
}
