using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.EstacionCQRS.DTO
{
    public class FormaPago
    {
        public string ClaveSAT { get; set; }
        public string Descripcion { get; set; }
        public int Estatus { get; set; }
        public int Orden { get; set; }
        public bool AplicaEstacion { get; set; }
        public int ClaveCG { get; set; }
        
    }
}
