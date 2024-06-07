using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GTCQRS.DTO
{
    public class AnticipoDTO
    {
    }
    public class AddAnticipoDTO
    {
        public int FechaCG { get; set; }
        public int Turno { get; set; }
        public int IslaId { get; set; }
        public int NoEmpleado { get; set; }
        public int FchCor { get; set; }
        public int IdTipoPago { get; set; }
        public double Cantidad { get; set; }
        public double Monto { get; set; }
    }
}
