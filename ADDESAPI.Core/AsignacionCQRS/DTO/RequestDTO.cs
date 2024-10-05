using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.AsignacionCQRS.DTO
{
    public class RequestDTO
    {

    }
    public class GetAsignacionesReqDTO
    {
        public string Fecha { get; set; }
        public int Turno { get; set; }
    }
    public class GetAsignacionReqDTO
    {
        public int NoEmpleado { get; set; }
    }
}
