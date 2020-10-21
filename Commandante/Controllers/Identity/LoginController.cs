using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Commandante.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Commandante.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<CommandanteUser> _userManager;
        private readonly IConfiguration _configuration;

        public LoginController(
            UserManager<CommandanteUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Credentials login)
        {
            var user = await _userManager.FindByNameAsync(login.Username);

            if (await _userManager.CheckPasswordAsync(user, login.Password))
            {
                string tokenString = await GenerateJSONWebTokenAsync(await _userManager.FindByNameAsync(login.Username));
                return Ok(new { token = tokenString });
            }
            else return Unauthorized("Invalid credentials.");
        }

        private async Task<string> GenerateJSONWebTokenAsync(CommandanteUser user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Program.JwtSecret));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(120),
                SigningCredentials = credentials,
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"]
            };
            foreach (string role in await _userManager.GetRolesAsync(user))
            {
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            foreach (Claim claim in await _userManager.GetClaimsAsync(user))
            {
                tokenDescriptor.Subject.AddClaim(claim);
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
