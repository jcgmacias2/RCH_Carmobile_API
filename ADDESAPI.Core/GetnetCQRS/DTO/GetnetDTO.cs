using ADDESAPI.Core.GetnetCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GetnetCQRS
{
    public class GetnetDTO
    {
    }
    public class GetnetTransaccionesCorteDTO
    {
        public int NoEmpleado { get; set; }
        public string Nombre { get; set; }
        public int Turno { get; set; }
        public double Debito { get; set; }
        public double Credito { get; set; }
        public double TotalTarjetas { get; set; }
        public double AMEX { get; set; }
        public double Diesel { get; set; }
        public double Devoluciones { get; set; }
        public List<ePagoTransaccion> Transacciones { get; set; }
        public List<ePagoTransaccion> TransaccionesDiesel { get; set; }
    }
}
