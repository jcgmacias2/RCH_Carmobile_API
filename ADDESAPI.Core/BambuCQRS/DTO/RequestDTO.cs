using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.BambuCQRS.DTO
{
    public class RequestDTO
    {
    }
    public class RequestFuelingQrDTO
    {
        public string user_id { get; set; }
        public string ticket { get; set; }
        public string charge { get; set; }
        public string fuel_type { get; set; }
        public string date { get; set; }
    }
    public class RequestGetQrDTO
    {
        public string QR { get; set; }
    }
    public class RequestSetQrDTO
    {
        public string QR { get; set; }
        public string Estacion { get; set; }
        public string Bomba { get; set; }
        public string NoEmpleado { get; set; }
    }
}
