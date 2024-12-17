using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.PresetCQRS.DTO
{
    public class Preset
    {
        public int Id { get; set; }
        public int Gasolinera { get; set; }
        public int Bomba { get; set; }
        public int UMedida { get; set; }
        public int Grado { get; set; }
        public double Cantidad { get; set; }
        public double Total { get; set; }
        public int NoEmpleado { get; set; }
        public string Vendedor { get; set; }
        public int IdTipoPago { get; set; }
        public string Moneda { get; set; }
        public int Estatus { get; set; }
        public string Error { get; set; }
        public string CardNumber { get; set; }
        public int LitrosRedimir { get; set; }
        public double Descuento { get; set; }
    }
    public class PresetDTO
    {
        public int Bomba { get; set; }
        public int Grado { get; set; }
        public double Cantidad { get; set; }
        public int UMedida { get; set; }
        public int NoEmpleado { get; set; }
        public string Nombre { get; set; }
        public int TipoPago { get; set; }
        public string RFC { get; set; }
        public double Total { get; set; }
        public double Descuento { get; set; }
        public string Moneda { get; set; }
        public int IdPreventa { get; set; }
        public double TipoCambio { get; set; }
        public double UsdRecibidos { get; set; }
        public double CambioUSD { get; set; }
        public double CambioMXN { get; set; }
        public string CardNumber { get; set; }
        public int LitrosRedimir { get; set; }
        public string BrandId { get; set; }
        public string ProgramId { get; set; }
        public string Cliente { get; set; }
        public string PromoDesc { get; set; }
        public string? Empresa { get; set; }
        public int? Producto { get; set; }
    }
    public class PresetGTDTO
    {
        public int Bomba { get; set; }
        public int Grado { get; set; }
        public double CantOimp { get; set; }
        public int CodDespachador { get; set; }
        public int LAD { get; set; }
        public int tiptrn { get; set; }
        public PresetClienteAppGTDTO ClientApp_RLS { get; set; }
    }
    public class PresetClienteAppGTDTO
    {
        //public int Estacion { get; set; }
        //public string QrPago { get; set; }
        //public string RFC { get; set; }
        //public string QrCupon { get; set; }
        //public string Cupon { get; set; }
        //public double Total { get; set; }
        //public double Descuento { get; set; }

        public int Tipo { get; set; }
        //public PresetCuponAppGTDTO cuponApp { get; set; }
        public PresetJarreoAppGTDTO jarreoApp { get; set; }
        public PresetDolaresAppGTDTO USD { get; set; }
        public PresetAcumularAppGTDTO acumular { get; set; }
        public PresetRedimirAppGTDTO redimir { get; set; }

    }
    //public class PresetCuponAppGTDTO
    //{
    //    public int Estacion { get; set; }
    //    public string QrPago { get; set; }
    //    public string RFC { get; set; }
    //    public string QrCupon { get; set; }
    //    public string Cupon { get; set; }
    //    public double Total { get; set; }
    //    public double Descuento { get; set; }
    //}
    public class PresetJarreoAppGTDTO
    {
        public int NoEstacion { get; set; }
        public int Gasolinera { get; set; }
        public int Transaccion { get; set; }
        public string Tipo { get; set; }
        public int Bomba { get; set; }
        public int CodProducto { get; set; }
        public string Producto { get; set; }
        public double Cantidad { get; set; }
        public double Importe { get; set; }
        public string FechaDespacho { get; set; }
        public string Usuario { get; set; }
    }
    public class PresetDolaresAppGTDTO
    {
        public int Estacion { get; set; }
        public int Gasolinera { get; set; }
        public int Transaccion { get; set; }
        public int NoEmpleado { get; set; }
        public string Nombre { get; set; }
        public int Turno { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string FechaCorte { get; set; }
        public int FechaCG { get; set; }
        public int Bomba { get; set; }
        public double ImporteDespacho { get; set; }
        public double DolaresRecibidos { get; set; }
        public double TipoCambio { get; set; }
        public double CambioUSD { get; set; }
        public double CambioMXN { get; set; }
        public List<PresetDolaresDetAppGTDTO> Detalle { get; set; }
    }
    public class PresetDolaresDetAppGTDTO
    {
        public int Estacion { get; set; }
        public int Gasolinera { get; set; }
        public int Transaccion { get; set; }
        public int Despacho { get; set; }
        public int Producto { get; set; }
        public double Cantidad { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public double Subtotal { get; set; }
        public double IVA { get; set; }
        public double IEPS { get; set; }
        public double Total { get; set; }
        public double CuotaIEPS { get; set; }
        public double TasaIVA { get; set; }
    }
    public class PresetAcumularAppGTDTO
    {
        public int Estacion { get; set; }
        public int Gasolinera { get; set; }
        public int Transaccion { get; set; }
        public string Fecha { get; set; }
        public int Bomba { get; set; }
        public double Total { get; set; }
        public int IdTipoPago { get; set; }
        public string CardNumber { get; set; }
        public int NoEmpleado { get; set; }
        public string Vendedor { get; set; }
        public string ProgramId { get; set; }
        public string BrandId { get; set; }
        public string Usuario { get; set; }
        public string Empresa { get; set; }
        public List<PresetDolaresDetAppGTDTO> Detalle { get; set; }
    }
    public class PresetAcumularDetAppGTDTO
    {
        public int Transaccion { get; set; }
        public int Producto { get; set; }
        public double Cantidad { get; set; }
        public string Descripcion { get; set; }
        public double Precio { get; set; }
        public double Total { get; set; }
    }
    public class PresetRedimirAppGTDTO
    {
        public int Estacion { get; set; }
        public int Gasolinera { get; set; }
        public int Transaccion { get; set; }
        public int Bomba { get; set; }
        public string CardNumber { get; set; }
        public int LitrosRedimir { get; set; }
        public double Cantidad { get; set; }
        public double Descuento { get; set; }
        public double Precio { get; set; }
        public double Total { get; set; }        
        public int NoEmpleado { get; set; }
        public string Vendedor { get; set; }
        public string BrandId { get; set; }
        public string ProgramId { get; set; }
        public string Usuario { get; set; }
        public int idPromotion { get; set; }
        public string Cliente { get; set; }
        public string PromoDesc { get; set; }
        public string Empresa { get; set; }
    }
}
