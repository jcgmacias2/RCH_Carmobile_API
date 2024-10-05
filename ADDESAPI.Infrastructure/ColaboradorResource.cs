using ADDESAPI.Core;
using ADDESAPI.Core.Colaborador;
using ADDESAPI.Core.Colaborador.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepoDb;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ADDESAPI.Infrastructure
{
    public class ColaboradorResource : IColaboradorResource
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly string _secretKey;
        public readonly string _audienceToken;
        public readonly string _issuerToken;
        public readonly string _expireTime;
        public readonly int _gasolinera;
        public readonly int _estacion;        

        public ColaboradorResource(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            _secretKey = _configuration["Token:JWT_SECRET_KEY"];
            _audienceToken = _configuration["Token:JWT_AUDIENCE_TOKEN"];
            _issuerToken = _configuration["Token:JWT_ISSUER_TOKEN"];
            _expireTime = _configuration["Token:JWT_EXPIRE_MINUTES"];
            _gasolinera = int.Parse(_configuration["Settings:Gasolinera"]);
            _estacion = int.Parse(_configuration["Settings:Estacion"]);
        }
        public async Task<ResultSingle<vColaborador>> Login(string usuario, string password)
        {
            ResultSingle<vColaborador> Result = new ResultSingle<vColaborador>();
            try
            {
                vColaborador colaborador = new vColaborador();
                using var connection = new SqlConnection(_connectionString);
                var req = await connection.QueryAsync<vColaborador>(r => r.Usuario == usuario && r.Password == password);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = "Usuario o contraseña incorrecto";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Colaborador encontrado";
                    Result.Data = req.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
        
        public async Task<ResultSingle<string>> GenerateTokenJwt(string username)
        {
            ResultSingle<string> Result = new ResultSingle<string>();
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                // create a claimsIdentity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) });

                var tokenHandler = new JwtSecurityTokenHandler();

                var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                   audience: _audienceToken,
                   issuer: _issuerToken,
                   subject: claimsIdentity,
                   notBefore: DateTime.UtcNow,
                   expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_expireTime)),
                   signingCredentials: signingCredentials);

                var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);

                Result.Success = true;
                Result.Error = "";
                Result.Message = "Token Generado";
                Result.Data = jwtTokenString;

            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "Error";
                Result.Message = ex.Message;
            }
            return Result;
        }

        public async Task<ResultMultiple<ColaboradoresDTO>> GetColaboradores()
        {
            ResultMultiple<ColaboradoresDTO> Result = new ResultMultiple<ColaboradoresDTO>();
            try
            {
                string sql = $"SELECT * FROM vColaborador WHERE NoEstacion = {_estacion}";

                using var connection = new SqlConnection(_connectionString);
                var req = await connection.ExecuteQueryAsync<ColaboradoresDTO>(sql);

                if (req == null || req.Count() == 0)
                {
                    Result.Success = false;
                    Result.Error = "";
                    Result.Message = "No se encontraron registros";
                }
                else
                {
                    Result.Success = true;
                    Result.Error = "";
                    Result.Message = "Registros encontrados";
                    Result.Data = req.ToList();
                }
            }
            catch (Exception ex)
            {
                Result.Success = false;
                Result.Error = "";
                Result.Message = ex.Message;
            }
            return Result;
        }
       
    }
}
