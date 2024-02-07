using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ProducoCQRS.DTO
{
    public class FamiliaGT
    {
        public int cod { get; set; }
        public string den { get; set; }
        public int tabsup { get; set; }
    }
    public class ProductoGT
    {
        public int indoct { get; set; }
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public string unidad { get; set; }
        public string codigoExterno { get; set; }
        public string codigoBarras { get; set; }
        public string codigoBarrasAlterno { get; set; }
        public int habilitado { get; set; }
        public int externo { get; set; }
        public PrecioGT precioProducto { get; set; }
    }
    public class PrecioGT
    {
        public double precioVenta { get; set; }
        public double tasaIVA { get; set; }
        public double cuotaIEPS { get; set; }
        public double costoVenta { get; set; }
    }
    public class ProductosGTDTO
    {
        public int tipoPago { get; set; }
        public string detailsPayment { get; set; }
        public List<ProductoGTDTO> products { get; set; }
    }
    public class ProductoGTDTO
    {
        public int amount { get; set; }
        public double total { get; set; }
        public ProductoApiGT product { get; set; }
    }
    public class ProductoApiGT
    {
        public int indoct { get; set; }
        public int codigo { get; set; }
        public string descripcion { get; set; }
        public string unidad { get; set; }
        public string codigoExterno { get; set; }
        public string codigoBarras { get; set; }
        public string codigoBarrasAlterno { get; set; }
        public int habilitado { get; set; }
        public int externo { get; set; }
        public PrecioGT precioProducto { get; set; }
    }
}
