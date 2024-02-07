using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.Asignacion.DTO
{
    public class vAsignacion
    {
        public int NumeroEmpleado { get; set; }
        public int IdBomba { get; set; }
        public string Bomba { get; set; }
        public int noIsla { get; set; }
        public DateTime Fecha { get; set; }
        public int Turno { get; set; }
        public int NoBomba { get; set; }
        public int Estacion { get; set; }
        public string Nombre { get; set; }
        public bool HayChecada { get; set; }
        public DateTime HoraChecada { get; set; }
    }
    public class AsignacionColaboradorTurno
    {
        public int Turno { get; set; }
        public List<AsignacionDTO> Asignacion { get; set; }
    }
    public class AsignacionDTO
    {
        public int IdBomba { get; set; }
        public string Bomba { get; set; }
        public int noIsla { get; set; }
        public int NoBomba { get; set; }
    }
}
