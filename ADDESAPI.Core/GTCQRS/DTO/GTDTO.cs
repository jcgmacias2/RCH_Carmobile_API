using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GTCQRS.DTO
{
    public class GTDTO
    {
    }
    public class GTLoginDTO
    {
        public string Token { get; set; }
    }
    public class GTCommandResponse
    {
        public string command { get; set; }
        public bool executed { get; set; }
        public string response { get; set; }
    }
    public class GTPresetResponse
    {
        public int respuesta { get; set; }
        
        public string mensaje { get; set; }
        public int nroMov { get; set; }
    }
    public class ApiGtSetTypeResponse
    {
        public bool edit { get; set; }
        public bool restart { get; set; }
    }
    public class ApiGtErrorResponse
    {
        public string error { get; set; }
    }

    public class ApiGtBombasResponse
    {
        public int bomba { get; set; }
        public int status { get; set; }
        public int numDigitPreset { get; set; }
        public int typeOfOperation { get; set; }
    }
    public class ApiGtRestartSGPMResponse
    {
        public bool respuesta { get; set; }
    }
    public class ApiGtCancelPresetResponse
    {
        public string respuesta { get; set; }
    }
    public class ApiGtAnticipoRes
    {
        public int fch { get; set; }
        public int nrotur { get; set; }
        public int codisl { get; set; }
        public int codres { get; set; }
        public int sec { get; set; }
        public int fchcor { get; set; }
        public int codval { get; set; }
        public double can { get; set; }
        public double mto { get; set; }
        public int codgas { get; set; }
        public int mcaacu { get; set; }
        public int codcli { get; set; }
        public int imp { get; set; }
    }
    public class DiscountDTO
    {
        public int idPromotion { get; set; }
        public string Promotion { get; set; }
        public string ClientApp { get; set; }
        public double preset_discountPerLiter { get; set; }
        public double pointsApp { get; set; }
        public double littersApp { get; set; }
        public string TaxData { get; set; }

    }
}
