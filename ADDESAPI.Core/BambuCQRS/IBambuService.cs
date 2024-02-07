using ADDESAPI.Core.BambuCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.BambuCQRS
{
    public interface IBambuService
    {
        //Task<ResultSingle<BambuTokenDTO>> GetToken();
        Task<Result> FuelingQR(RequestFuelingQrDTO request);
        Task<ResultSingle<QR>> GetQrPago(RequestGetQrDTO request);
        Task<ResultSingle<QR>> SetQrPago(RequestSetQrDTO request);
        Task<Result> CancelQrPago(RequestGetQrDTO request);
    }
}
