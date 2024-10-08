﻿using ADDESAPI.Core.Asignacion.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.Asignacion
{
    public  interface IAsignacionResource
    {
        Task<ResultMultiple<vAsignacion>> GetAsignacion(int noEmpleado, DateTime fecha);
        Task<ResultMultiple<vAsignacion>> GetAsignacion(int noEmpleado, DateTime fecha, int turno);
        Task<ResultMultiple<AsignacionColaboradorDTO>> GetAsignaciones(string fecha, int turno);
    }
}
