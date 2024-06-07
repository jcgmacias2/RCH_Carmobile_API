using ADDESAPI.Core;
using ADDESAPI.Core.GetnetCQRS;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Infrastructure
{
    public class GetnetResource : IGetnetResource
    {
        private readonly IConfiguration _configuration;
        private readonly string _url;
        private readonly string _userAddes;
        private readonly string _pwdAddes;
        private readonly string _apiGetTransaccionesGetnet;
        public readonly int _estacion;
        public GetnetResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration["ADDES:URL"];
            _userAddes = _configuration["ADDES:UsrAddes"];
            _pwdAddes = _configuration["ADDES:PwdAddes"];
            _apiGetTransaccionesGetnet = _configuration["ADDES:ApiGetTransaccionesGetnet"];
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
        }
        public async Task<ResultSingle<GetnetTransaccionesCorteDTO>> GetTransaccionesTurnoVendedor(string fecha, int turno, int noEmpleado)
        {
            ResultSingle<GetnetTransaccionesCorteDTO> Result = new ResultSingle<GetnetTransaccionesCorteDTO>();
            try
            {

                var request = new RestRequest(_apiGetTransaccionesGetnet, Method.Post);

                var options = new RestClientOptions(_url)
                {
                    Authenticator = new HttpBasicAuthenticator(_userAddes, _pwdAddes)
                };
                var client = new RestClient(options);

                var Data = new
                {
                    Fecha = fecha,
                    Estacion = _estacion,
                    Turno = turno,
                    NoEmpleado = noEmpleado
                };

                request.AddJsonBody(new { Data = Data });
                request.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultADDES<GetnetTransaccionesCorteDTO>>(response.Content);

                    Result.Success = r.Success;
                    if (!r.Success)
                    {
                        Result.Error = $"Error al obtener las transacciones de Getnet {r.Error}";
                        Result.Message = $". Respuesta: {r.Message}";
                    }
                    else
                    {
                        Result.Error =r.Error;
                        Result.Message = r.Message;
                        Result.Data = r.response;
                    }
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error al obtener las transacciones de Getnet ";
                    Result.Message = $"Error de conexión. {_url} EstatusCode: {response.StatusCode } {response.StatusDescription}";
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = $"Error {ex.Message}";
            }
            return Result;
        }
    }
}
