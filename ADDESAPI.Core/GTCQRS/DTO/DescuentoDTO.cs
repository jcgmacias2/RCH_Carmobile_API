using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GTCQRS.DTO
{
    public class DescuentoDTO
    {
        public int Estacion { get; set; }
        public int Gasolinera { get; set; }
        public int Transaccion { get; set; }
        public double Descuento { get; set; }
        public string CardNumber { get; set; }
        public string NombreCliente { get; set; }
        public double litrosRedimidos { get; set; }
        public string PromoDesc { get; set; }
        public int PromoCode { get; set; }
        public int NoEmpleado { get; set; }
        public string Vendedor { get; set; }
    }
}
