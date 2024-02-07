using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ProducoCQRS.DTO
{
    public class vProductos
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Unidad { get; set; }
        public string CodigoExterno { get; set; }
        public string CodigoBarras { get; set; }
        public int Habilitado { get; set; }
        public double Precio { get; set; }
        public double TasaIVA { get; set; }
        public double CuotaIEPS { get; set; }
        public int IdFamilia { get; set; }
    }
}
