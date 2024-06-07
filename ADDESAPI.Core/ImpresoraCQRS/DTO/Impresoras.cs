using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ImpresoraCQRS.DTO
{
    public class Impresoras
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Bomba { get; set; }

        public string IP { get; set; }
        public int Estatus { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Marca { get; set; }
        public string modelo { get; set; }
    }
}
