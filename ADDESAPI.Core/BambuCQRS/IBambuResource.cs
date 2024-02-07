using ADDESAPI.Core.BambuCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.BambuCQRS
{
    public interface IBambuResource
    {
        Task<ResultSingle<BambuTokenDTO>> GetToken();
        Task<Result> FuelingQR(RequestFuelingQrDTO request, string token);
        Task<ResultSingle<QR>> GetQrPago(RequestGetQrDTO request, string token);
        Task<ResultSingle<QR>> SetQrPago(RequestSetQrDTO request, string token);
        Task<Result> CancelQrPago(RequestGetQrDTO request, string token);

    }
}
