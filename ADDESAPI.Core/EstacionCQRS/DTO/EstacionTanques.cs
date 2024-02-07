using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.EstacionCQRS.DTO
{
    public class EstacionTanques
    {
        public int CodigoTanque { get; set; }
        public int NumeroTanque { get; set; }
        public int Estacion { get; set; }
        public int Gasolinera { get; set; }
        public int CodigoProducto { get; set; }
        public string Producto { get; set; }
        public int GradoProducto { get; set; }
        public double CapacidadMaxima { get; set; }
        public double CapacidadOperacional { get; set; }
        public double CapacidadMinima { get; set; }
        public double CapacidadCritica { get; set; }
        public double CapacidadMaxH2O { get; set; }
        public double CapacidadTanque { get; set; }
        public double CapacidadEmergencia { get; set; }
        public double CapacidadEmergenciaH2O { get; set; }
        public double Precio { get; set; }
    }
}
