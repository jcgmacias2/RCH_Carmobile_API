using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ProducoCQRS.DTO
{
    public class ProductoReqDTO
    {
        public int Familia { get; set; }
    }
    public class ProductoCodigoReqDTO
    {
        public string Codigo { get; set; }
    }
    public class ProductoDTO
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
    public class ProductoRequestDTO
    {
        public int Bomba { get; set; }
        public int TipoPago { get; set; }
        public int NoEmpleado { get; set; }
        public List<Producto> Productos { get; set; }
    }
}
