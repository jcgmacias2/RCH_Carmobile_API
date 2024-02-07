using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.VentukCQRS.DTO
{
    public class Asistencia
    {
        public int ID_Empleado { get; set; }
        public bool HayChecadas { get; set; }
        public string DescripcionEstacion { get; set; }
        public DateTime? HoraChecada { get; set; }
        public string CodigoEstacion { get; set; }
    }
}
