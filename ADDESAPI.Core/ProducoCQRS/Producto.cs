using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ProducoCQRS
{
    public class Producto
    {
        public int Cantidad { get; set; }
        public double Total { get; set; }
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Unidad { get; set; }
        public string CodigoExterno { get; set; }
        public double Precio { get; set; }
        public double TasaIVA { get; set; }
    }
}
