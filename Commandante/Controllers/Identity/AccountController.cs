using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Commandante.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Commandante.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private const string DefaultUserName = "admin";
        private const string DefaultPassword = "admin";
        private readonly UserManager<CommandanteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<CommandanteUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Route("init")]
        [HttpGet]
        public async Task<IActionResult> Initialize()
        {
            if (_userManager.Users.Any())
            {
                return BadRequest("Server has already been initialized.");
            }
            var result = await _userManager.CreateAsync(new CommandanteUser(DefaultUserName), DefaultPassword);
            if (result.Succeeded)
            {
                result = await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(DefaultUserName);
                    result = await _userManager.AddToRoleAsync(user, Roles.Admin);
                    if (result.Succeeded)
                    {
                        return Ok($"Successfully initialized.\nUser: {DefaultUserName}\nPassword: {DefaultPassword}");
                    }
                }
            }
            return Problem(GetErrorMessage(result));
        }

        [Route("create")]
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateUser([FromBody] Credentials credentials)
        {
            var result = await _userManager.CreateAsync(new CommandanteUser(credentials.Username), credentials.Password);
            if (result.Succeeded)
            {
                return Ok(await _userManager.FindByNameAsync(credentials.Username));
            }
            else return Problem(GetErrorMessage(result));
        }

        [Route("makeAdmin")]
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> MakeAdmin([FromBody] string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            var result = await _userManager.AddToRoleAsync(user, Roles.Admin);
            if (result.Succeeded)
            {
                return Ok();
            }
            else return Problem(GetErrorMessage(result));
        }

        [Route("delete")]
        [HttpDelete]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DeleteUser([FromBody] string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("User not found.");
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }
            else return Problem(GetErrorMessage(result));
        }

        [Route("get")]
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<CommandanteUser> GetUser(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }

        private static string GetErrorMessage(IdentityResult result)
        {
            return string.Join("\n", result.Errors.Select(x => $"{x.Code}: {x.Description}"));
        }
    }
}
