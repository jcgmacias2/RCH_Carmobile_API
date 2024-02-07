using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GTCQRS.DTO
{
    public class PreciosGtDTO
    {
        //public List<PrecioBombaGtDTO> Precios { get; set; }
    }
    public class PrecioGtReqDTO
    {
        public int Bomba { get; set; }
    }
    public class PrecioBombaGtDTO
    {
        public bool Executed { get; set; }
        public string Response { get; set; }
        public int Bomba { get; set; }
        public int CodigoProducto { get; set; }
        public int Grado { get; set; }
        public string Descipcion { get; set; }
        public double Precio { get; set; }
    }
}
