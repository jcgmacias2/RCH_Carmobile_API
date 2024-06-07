using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.TanqueCQRS.DTO
{
    public class LecturasDTO
    {
        public string Estacion { get; set; }
        public string RFC { get; set; }
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaImpresion { get; set; }
        public List<LecturaDTO> Lecturas { get; set; }
    }
    public class LecturaDTO
    {
        public int CodProducto { get; set; }
        public string Producto { get; set; }
        public int Transaccion { get; set; }
        public int Gasolinera { get; set; }
        public int NumeroTanque { get; set; }
        public int CodigoTanque { get; set; }        
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public double Volumen { get; set; }
        public double volCxT { get; set; }
        public double VolumenH2O { get; set; }
        public double VolumenRecepcion { get; set; }
        public double Temperatura { get; set; }
        public double Porcentaje { get; set; }
        public double PorLlenar { get; set; }
        public int Documento { get; set; }
    }
}
