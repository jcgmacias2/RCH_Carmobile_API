using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.CorteCQRS.DTO
{
    public class CorteDTO
    {
        public double CantidadCombustible { get; set; }
        public double ImporteCombustible { get; set; }
        public double CantidadProductos { get; set; }
        public double ImporteProductos { get; set; }
        public double ImporteTotal { get; set; }
        public double ImporteFajillasMXN { get; set; }
        public double ImporteFajillasUSD { get; set; }
        public double ImporteTDC { get; set; }
        public double ImporteTDD { get; set; }
        public double ImporteAMEX { get; set; }
        public double ImporteTarjetasDiesel { get; set; }
        public double ImporteDevolucionesTarjetas { get; set; }
        public double ImporteMonederos { get; set; }
        public double ImporteJarreos { get; set; }
        public double TipoCambio { get; set; }
    }
}
