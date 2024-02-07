using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.FajillaCQRS.DTO
{
    public class ReqFajillas
    {
        public string Fecha { get; set; }
        public int Estacion { get; set; }
        public int NoEmpleado { get; set; }
        public int Turno { get; set; }
    }
}
