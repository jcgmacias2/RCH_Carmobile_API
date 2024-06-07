using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.VentaCQRS
{
    public class vVenta
    {
        public DateTime Fecha { get; set; }
        public int Gasolinera { get; set; }
        public int Turno { get; set; }
        public int Isla { get; set; }
        public string Bomba { get; set; }
        public int NoBomba { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public double Cantidad { get; set; }
        public double Total { get; set; }
        public int NoEmpleado { get; set; }
        public int TipoPago { get; set; }
    }
}
