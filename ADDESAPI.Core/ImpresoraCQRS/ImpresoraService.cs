using ADDESAPI.Core.ImpresoraCQRS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Core.ImpresoraCQRS
{
    public class ImpresoraService : IImpresoraService
    {
        private readonly IImpresoraResource _resource;

        public ImpresoraService(IImpresoraResource resource)
        {
            _resource = resource;
        }
        public async Task<ResultMultiple<Impresoras>> GetImpresoras()
        {
            ResultMultiple<Impresoras> Result = new ResultMultiple<Impresoras>();
            try
            {

                Result = await _resource.GetImpresoras();


            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error al obtener las impresoras";
                Result.Message = $"{ex.Message}";
            }
            return Result;
        }
    }
}
