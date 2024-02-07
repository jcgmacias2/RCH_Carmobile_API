using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.EstacionCQRS.DTO
{
    public class EstacionCombustiblesDTO
    {
        public int CodigoProducto { get; set; }
        public string Producto { get; set; }
        public int GradoProducto { get; set; }
    }
}
