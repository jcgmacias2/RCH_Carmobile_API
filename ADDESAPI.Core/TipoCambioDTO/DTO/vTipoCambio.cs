using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.TipoCambioDTO.DTO
{
    public class vTipoCambio
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public double TC { get; set; }
        public int Estacion { get; set; }
    }
}
