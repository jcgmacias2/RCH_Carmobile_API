using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GTCQRS.DTO
{
    public class GTDTO
    {
    }
    public class GTLoginDTO
    {
        public string Token { get; set; }
    }
    public class GTCommandResponse
    {
        public string command { get; set; }
        public bool executed { get; set; }
        public string response { get; set; }
    }
    public class GTPresetResponse
    {
        public int respuesta { get; set; }
        
        public string mensaje { get; set; }
        public int nroMov { get; set; }
    }

}
