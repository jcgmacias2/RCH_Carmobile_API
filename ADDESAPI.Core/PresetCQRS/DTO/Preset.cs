using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.PresetCQRS.DTO
{
    public class Preset
    {
    }
    public class PresetDTO
    {
        public int Bomba { get; set; }
        public int Grado { get; set; }
        public double Cantidad { get; set; }
        public int UMedida { get; set; }
        public int NoEmpleado { get; set; }
        public int TipoPago { get; set; }
        public string QrPago { get; set; }
        public string RFC { get; set; }
        public string QrCupon { get; set; }
        public string Cupon { get; set; }
        public double Total { get; set; }
        public double Descuento { get; set; }
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
        public int Estacion { get; set; }
        public string QrPago { get; set; }
        public string RFC { get; set; }
        public string QrCupon { get; set; }
        public string Cupon { get; set; }
        public double Total { get; set; }
        public double Descuento { get; set; }
    }
}
