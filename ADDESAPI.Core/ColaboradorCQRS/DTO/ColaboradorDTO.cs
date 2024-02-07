using ADDESAPI.Core.Asignacion.DTO;
using ADDESAPI.Core.VentukCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ADDESAPI.Core.Colaborador.DTO
{
    public class ColaboradorDTO
    {
        public int NumeroVentuk { get; set; }
        public string Nombre { get; set; }
        public int Estatus { get; set; }
        public int NoEstacion { get; set; }
        public string Estacion { get; set; }
        public string Puesto { get; set; }
        public string ROL { get; set; }
        public string Token { get; set; }
        public int IdRol { get; set; }
        public List<AsignacionColaboradorTurno> Asignacion { get; set; }
        public Asistencia AsistenciaVentuk { get; set; }
    }

}
