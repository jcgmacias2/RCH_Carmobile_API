using ADDESAPI.Core;
using ADDESAPI.Core.VentukCQRS;
using ADDESAPI.Core.VentukCQRS.DTO;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Infrastructure
{
    public class VentukResource : IVentukResource
    {
        private readonly IConfiguration _configuration;
        private readonly string _url;
        private readonly string _tokenAPI;
        private readonly string _apiAsistencia;

        public VentukResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _url = _configuration["Ventuk:URL"];
            _tokenAPI = _configuration["Ventuk:TokenAPI"];
            _apiAsistencia = _configuration["Ventuk:ApiAsistencia"];
        }

        public async Task<ResultSingle<Asistencia>> GetAsistencia(int idEmpleado, DateTime fecha)
        {
            ResultSingle<Asistencia> Result = new ResultSingle<Asistencia>();
            try
            {
                var client = new RestClient(_url);
                var requestV = new RestRequest(_apiAsistencia, Method.Post);
                var Data = new
                {
                    Fecha = fecha.ToString("yyyy-MM-dd"),
                    ID_Empleado = idEmpleado,
                    TokenAPI = _tokenAPI
                };
                requestV.AddJsonBody(Data);
                requestV.RequestFormat = DataFormat.Json;

                var response = client.Execute<ResultSingle<string>>(requestV);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultVentuk<Asistencia>>(response.Content);

                    if (!r.Status)
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"Error al consultar la checada en Ventuk";
                    }

                    if (r.Data == null)
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"No se encontro información del vendedor en Ventuk";
                    }

                    List<Asistencia> Asistencias = new List<Asistencia>();
                    Asistencias = r.Data.ToList();

                    if (Asistencias.Count() > 0)
                    {
                        var Asistencia = Asistencias.OrderBy(a => a.HoraChecada).FirstOrDefault();

                        if (Asistencia.HayChecadas)
                        {
                            Result.Success = true;
                            Result.Error = "";
                            Result.Message = $"Checada encontrada";
                            Result.Data = Asistencia;
                        }
                        else
                        {
                            Result.Success = false;
                            Result.Error = "Error";
                            Result.Message = $"No se encontro Checada";
                        }
                    }
                    else
                    {
                        Result.Success = false;
                        Result.Error = "Error";
                        Result.Message = $"No se encontro la checada del vendedor en Ventuk";
                    }
                }
                else
                {
                    Result.Success = false;
                    Result.Error = "Error";
                    Result.Message = $"Error de conexion a Ventuk {_url} EstatusCode: {response.StatusCode }{response.StatusDescription}";
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
