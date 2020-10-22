using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Commandante.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Commandante.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelfController : ControllerBase
    {
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("update")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Update(IFormFile zip)
        {
            if (!System.IO.File.Exists(Program.UpdaterExecutableFilePath))
            {
                return BadRequest($"Updater is not installed in {Program.UpdaterExecutableFilePath}.");
            }
            string serviceName = Program.ServiceName;
            if (serviceName == null)
            {
                return BadRequest("Service name is not set in registry.");
            }
            string filePath = Path.Combine(Program.AppData.FullName, "update.zip");
            string serviceDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            using var stream = new FileStream(filePath, FileMode.Create);
            await zip.CopyToAsync(stream);
            ProcessStartInfo psi = new ProcessStartInfo
            {
                Arguments = $"{serviceName} {filePath} {serviceDirectory}",
                FileName = Program.UpdaterExecutableFilePath
            };
            Process.Start(psi);
            return Ok();
        }
    }
}
