using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.FajillaCQRS.DTO
{
    public class vFajillas
    {
        public int NoEstacion { get; set; }
        public string Estacion { get; set; }
        public int NoEmpleado { get; set; }
        public string Nombre { get; set; }
        public int Folio { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaHora { get; set; }
        public double Monto { get; set; }
        public int Turno { get; set; }
        public int Bomba { get; set; }
        public int Isla { get; set; }
        public string Moneda { get; set; }
        public int IdCajero { get; set; }
        public string Cajero { get; set; }
        public DateTime FechaAutorizacion { get; set; }
        public int Estatus { get; set; }
        public int IdValor { get; set; }
        public string EstatusDesc { get; set; }
    }
}
