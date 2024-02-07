using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.DespachosCQRS.DTO
{
    public class vDespachos
    {
        public int Transaccion { get; set; }
        public int Gasolinera { get; set; }
        public int NoEstacion { get; set; }
        public string Estacion { get; set; }
        public int Turno { get; set; }
        public int FechaCG { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public int Bomba { get; set; }
        public string TipoPago { get; set; }
        public int IdTipoPago { get; set; }
        public string FormaPagoSAT { get; set; }        
        public double Cantidad { get; set; }
        public double Precio { get; set; }
        public double Total { get; set; }
        public int Cliente { get; set; }
        public string UUID { get; set; }
        public int Producto { get; set; }
        public string Descripcion { get; set; }
        public string Unidad { get; set; }
        public DateTime lognew { get; set; }
        public string PermisoCRE { get; set; }
        public int Despacho { get; set; }        
        public string ClaveProdServ { get; set; }
        public string ClaveUnidad { get; set; }
        public string RFC { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string DomicilioFiscal { get; set; }
    }
}
