using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiRestFactura.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiRestFactura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacionController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AutenticacionController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        [HttpPost]
        [Route("GenerarToken")]
        public IActionResult GenerarToken([FromBody] Peticion peticion)
        {
            if (ModelState.IsValid)
            {
                //Se simula una validacion de usuario existente
                if (peticion.Usuario.Equals("usuario") && peticion.Contrasenia.Equals("contrasenia"))
                {
                    return BuildToken(peticion);
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //Genera el Token
        private IActionResult BuildToken(Peticion peticion)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, peticion.Usuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //Se puede colocar un identificador unico al token
                new Claim("ParametroAdicional", "ValorParametroAdicional")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Secret:Key"])); //Esta configurada en las variables de ambiente de la aplicacion
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(2);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: "yourdomain.com", //Quien emite el token
               audience: "yourdomain.com", //Para quien va dirigido el token
               claims: claims, //Informacion adicional que se puede considerar que es confiable 
               expires: expiration, //Tiempo de expiracion del token
               signingCredentials: creds); //Llave encriptada

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration
            });

        }
    }
}