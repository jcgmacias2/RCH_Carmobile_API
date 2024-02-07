using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.CorteCQRS.DTO
{
    public class RequestCorteDTO
    {
        public string Fecha { get; set; }
        public int NoEmpleado { get; set; }
        public int Estacion { get; set; }
        public int Turno { get; set; }
        public int Bomba { get; set; }
    }
}
