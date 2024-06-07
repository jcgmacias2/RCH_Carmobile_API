using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.PreventaCQRS.DTO
{
    public class Preventa
    {
        public int Id { get; set; }
        public int Gasolinera { get; set; }
        public int Tipo { get; set; }
        public string DescripcionTipo { get; set; }
        public string Moneda { get; set; }
        public double TipoCambio { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public int Bomba { get; set; }
        public int TipoPago { get; set; }
        public double Cantidad { get; set; }
        public double Importe { get; set; }
        public string Usuario { get; set; }
        public DateTime Fecha { get; set; }
        public int Turno { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int Estatus { get; set; }
    }
}
