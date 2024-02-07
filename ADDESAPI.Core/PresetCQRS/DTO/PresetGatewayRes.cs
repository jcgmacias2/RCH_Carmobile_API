using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.PresetCQRS.DTO
{
    public class PresetGatewayRes
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public int NoBomba { get; set; }
        public int DespachoAnterior { get; set; }
        public string Preset { get; set; }
        public string RespuestaGateway { get; set; }
    }
}
