using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.BambuCQRS.DTO
{
    public class BambuDTO
    {
    }
    public class BambuTokenDTO
    {
        public bool valid { get; set; }
        public string token { get; set; }
        public string session_id { get; set; }
        public string id_user { get; set; }
        public bool verified { get; set; }
        public string error_code { get; set; }
        public string error { get; set; }
        public string message { get; set; }
    }
    public class BambuErrorDTO
    {
        public bool valid { get; set; }
        public string error_code { get; set; }
        public string error { get; set; }
        public string message { get; set; }
    }

    public class BambuFuelingDTO
    {
        public bool valid { get; set; }
        public fueling fueling { get; set; }
        public string error_code { get; set; }
        public string error { get; set; }
        public string internal_message { get; set; }
        public string message { get; set; }
    }
    public class fueling
    {
        public string id_fueling { get; set; }
        public string payment_reference { get; set; }
        public string qr { get; set; }
        public double price_per_liter { get; set; }
        public double liters { get; set; }
        public double total { get; set; }
        public double discount { get; set; }
        public double full_discount { get; set; }
        public string payment_method { get; set; }
        public string payment_folio { get; set; }
        public string finalization { get; set; }
        public FuelType fuel_type { get; set; }
        public branch_office branch_Office { get; set; }
        public succeded_at succeded_at { get; set; }
        
    }
    public class branch_office
    {
        public int id_branch { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string branch_longitud { get; set; }
        public string branch_latitud { get; set; }
    }
    public class FuelType
    {
        public string value { get; set; }
    }
    public class succeded_at
    {
        //public DateTime date { get; set; }
        //public TimeSpan time { get; set; }
        public DateTime fulldate { get; set; }
    }
    public class QR
    {
        public int EstacionPago { get; set; }
        public string Combustible { get; set; }
        public string FormaPago { get; set; }
        public string ReferenciaPago { get; set; }
        public double Precio { get; set; }
        public double Litros { get; set; }
        public double Total { get; set; }
        public double Descuento { get; set; }
        public double TotalConDescuento { get; set; }
        public DateTime FechaCupon { get; set; }
    }
    public class CancelQr
    {
        public bool valid { get; set; }
        public string message { get; set; }
    }
}
