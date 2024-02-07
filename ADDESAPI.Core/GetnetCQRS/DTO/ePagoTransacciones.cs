using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.GetnetCQRS.DTO
{
    public class ePagoTransacciones
    {
        public List<ePagoTransaccion> Transacciones { get; set; }
    }
    public class ePagoTransaccion
    {
        public string nu_operaion { get; set; }
        public string cd_usuario { get; set; }
        public string cd_empresa { get; set; }
        public string nu_sucursal { get; set; }
        public string nu_afiliacion { get; set; }
        public string nb_referencia { get; set; }
        public string cc_nombre { get; set; }
        public string cc_num { get; set; }
        public string cc_tp { get; set; }
        public string nu_importe { get; set; }
        public string cd_tipopago { get; set; }
        public string cd_tipocobro { get; set; }
        public string cd_instrumento { get; set; }
        public string nb_response { get; set; }
        public string nu_auth { get; set; }
        public string fh_registro { get; set; }
        public string fh_bank { get; set; }
        public string cd_usrtransaccion { get; set; }
        public string tp_operacion { get; set; }
        public string nb_currency { get; set; }
        public string cd_resp { get; set; }
        public string nb_resp { get; set; }
    }
}
