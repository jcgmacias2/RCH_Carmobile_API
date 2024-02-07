using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ADDESAPI.Core.EstacionCQRS.DTO
{
    public class vBombas
    {
        public int Id { get; set; }
        public string Bomba { get; set; }
        public int Isla { get; set; }
        public int Numero { get; set; }
        [JsonIgnore]
        public int Estacion { get; set; }
    }
}
