using ApiOAuthPractica.Helpers;
using ApiOAuthPractica.Models;
using ApiOAuthPractica.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ApiOAuthPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryDoctores repo;

        private HelperActionServicesOAuth helper;

        public AuthController(RepositoryDoctores repo, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            Doctor doctor = await this.repo.LoginDoctorAsync(model.Username, int.Parse(model.Password));
            if (doctor == null)
            {
                return Unauthorized();

            }
            else
            {
                SigningCredentials credentials = new SigningCredentials(
                    this.helper.GetKeyToken()
                    , SecurityAlgorithms.HmacSha256);

                //PONEMOS EL EMPLEADO EN FORMATO JSON
                string jsonDoctor = JsonConvert.SerializeObject(doctor);

                //CREAMOS UN ARRAY CON TODA LA INFORMACION QUE QUEREMOS GUARDAR EN EL TOKEN
                Claim[] informacion = new[]
                {
                    new Claim("UserData", jsonDoctor)
                };


                JwtSecurityToken token = new JwtSecurityToken(
                    claims: informacion,
                    issuer: this.helper.Issuer,
                    audience: this.helper.Audience,
                    signingCredentials: credentials,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    notBefore: DateTime.UtcNow
                    );

                //POR ULTIMO, DEVOLVEMOS UNA RESPUESTA AFIRMATIVA 
                //CON UN OBJETO ANONIMO EN FORMATO JSON 
                return Ok(
                    new
                    {
                        response = new JwtSecurityTokenHandler().WriteToken(token)
                    });

            }

        }



    }

}
