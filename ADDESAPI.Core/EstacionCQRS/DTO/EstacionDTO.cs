using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.EstacionCQRS.DTO
{
    public class EstacionDTO
    {
        public int NoEstacion { get; set; }
        public string Nombre { get; set; }
        public int CiudadId { get; set; }
        public string Direccion { get; set; }
        public string RFC { get; set; }
        public string RazonSocial { get; set; }
        public string DomicilioFiscal { get; set; }
        public string IP { get; set; }
        public string CP { get; set; }
    }

}
