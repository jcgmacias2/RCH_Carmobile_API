using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.DespachosCQRS.DTO
{
    public class RedemptionDTO
    {
        public double LitrosRedimidos { get; set; }
        public double Descuento { get; set; }
        public double Balance { get; set; }
        public double PreviousBalance { get; set; }
        public double Redimidos { get; set; }
    }
    public class RedemptionReq
    {
        public int Transaccion { get; set; }
        public int Estacion { get; set; }
        public int Gasolinera { get; set; }
        public int Bomba { get; set; }
        public string CardNumber { get; set; }
        public int LitrosRedimir { get; set; }
        public double Cantidad { get; set; }
        public double Total { get; set; }
        public int NoEmpleado { get; set; }
        public string Vendedor { get; set; }
        public double Descuento { get; set; }
        public double Precio { get; set; }
        public int IdTipoPago { get; set; }
        public int FechaCG { get; set; }
        public int FchCor { get; set; }
        public int Turno { get; set; }
        public int IslaId { get; set; }
        public string BrandId { get; set; }
        public string ProgramId { get; set; }
        public string Usuario { get; set; }
        public int Producto { get; set; }
        public string Cliente { get; set; }
        public string PromoDesc { get; set; }
        public string? Empresa { get; set; }
    }
}
