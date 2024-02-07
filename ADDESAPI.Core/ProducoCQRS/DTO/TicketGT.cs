using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ProducoCQRS.DTO
{
    public class TicketGT
    {
        public int folio { get; set; }
        public int codgas { get; set; }
        public int fecha { get; set; }
        public int fechaCorte { get; set; }
        public int numeroTurno { get; set; }
        public int codIsla { get; set; }
        public int codResponsable { get; set; }
        public string datRef { get; set; }
        public string fechaStr { get; set; }
        public int hora { get; set; }
        public string horaStr { get; set; }
        public int posicion { get; set; }
        public int codigoProducto { get; set; }
        public string producto { get; set; }
        public string unidadMedida { get; set; }
        public string cvePemex { get; set; }
        public double cantidad { get; set; }
        public double precio { get; set; }
        public double importe { get; set; }
        public TicketGasolineraGT gasolinera { get; set; }
    }
    public class TicketGasolineraGT
    {
        public int cod { get; set; }
        public string numES { get; set; }
        public string denominacion { get; set; }
        public string nombreES { get; set; }
        public string domicilio { get; set; }
        public string colonia { get; set; }
        public string municipio { get; set; }
        public string ciudad { get; set; }
        public string estado { get; set; }
        public string pais { get; set; }
        public string rfc { get; set; }
        public string claveSIIC { get; set; }
        public string permisoCREComer { get; set; }
        public string regFiscal { get; set; }
        public string regFiscalDen { get; set; }
        public string codigoPostal { get; set; }
        public string telefono { get; set; }
    }
}
