using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.EstacionCQRS.DTO
{
    public class vGasolinera
	{
        public int Codigo { get; set; }
		public string Nombre { get; set; }
		public string Denominacion { get; set; }
		public int TurnoActual { get; set; }
		public string CodigoExterno { get; set; }
		public string Clave { get; set; }
		public string PermisoCRE { get; set; }
		public string CP { get; set; }
        public DateTime Fecha { get; set; }

    }
	public class TurnoActualDTO
	{
        public int Turno { get; set; }
    }
}
