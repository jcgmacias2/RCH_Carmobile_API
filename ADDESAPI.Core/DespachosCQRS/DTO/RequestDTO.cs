using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.DespachosCQRS.DTO
{
    public class RequestDTO
    {
    }
    public class RequestTransaccionDTO
    {
        public string Transaccion { get; set; }
    }
    public class RequestTransaccionesDTO
    {
        public string Bomba { get; set; }
    }
    public class ReqTransaccionTpDTO
    {
        public int Transaccion { get; set; }
        public int TipoPago { get; set; }
    }
}
