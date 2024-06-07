using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.DespachosCQRS.DTO
{
    public class DespachoDTO
    {
        public int Transaccion { get; set; }
        public int Gasolinera { get; set; }
        public int NoEstacion { get; set; }
        public string Estacion { get; set; }
        public string Ticket { get; set; }
        public string RFC { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string DomicilioFiscal { get; set; }
        public string PermisoCre { get; set; }
        public int Turno { get; set; }
        public int FechaCG { get; set; }
        public int FchCor { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaCorte { get; set; }
        public string Hora { get; set; }
        public int Bomba { get; set; }
        public int IslaID { get; set; }
        public double Subtotal { get; set; }
        public double IVA { get; set; }
        public double IEPS { get; set; }
        public double Total { get; set; }
        public int IdTipoPago { get; set; }
        public string TipoPago { get; set; }
        public string FormaPagoSAT { get; set; }
        public int Cliente { get; set; }
        public string UUID { get; set; }
        public string DescProductos { get; set; }
        public bool VentaApp { get; set; }
        public int TipoPagoApp { get; set; }        
        public string WebID { get; set; }
        public double Descuento { get; set; }
        public string CardNumber { get; set; }
        public string NombreCliente { get; set; }
        public double Puntos { get; set; }
        public double litrosRedimidos { get; set; }
        public string PromoDesc { get; set; }
        public int PromoCode { get; set; }
        public int NoEmpleado { get; set; }
        public string Vendedor { get; set; }
        public List<DespachoDetalleDTO> Detalle { get; set; }
    }
    public class DespachoDetalleDTO
    {
        public int Transaccion { get; set; }
        public int Producto { get; set; }
        public double Cantidad { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public double ValorUnitario { get; set; }
        public double Subtotal { get; set; }
        public double IVA { get; set; }
        public double IEPS { get; set; }
        public double Total { get; set; }
        public double CuotaIEPS { get; set; }
        public double TasaIVA { get; set; }
        public string ClaveProdServ { get; set; }
        public string ClaveUnidad { get; set; }
        public string Unidad { get; set; }

    }
    public class DespachoAppDTO
    {
        public string Transaccion { get; set; }
        public int Gasolinera { get; set; }
        public int Turno { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public int Bomba { get; set; }
        public double Total { get; set; }
        public int IdTipoPago { get; set; }
        public string TipoPago { get; set; }
        public string Descripcion { get; set; }
        public int Productos { get; set; }
        public List<DespachoDetalleAppDTO> Detalle { get; set; }

    }
    public class DespachoDetalleAppDTO
    {
        public string Transaccion { get; set; }
        public int Producto { get; set; }
        public double Cantidad { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public double Total { get; set; }
        public string Despacho { get; set; }
    }
}
