﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Commandante.Data;
using Commandante.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Commandante.Controllers.Processes
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly CommandanteContext _context;

        public ProjectsController(ILogger<ProjectsController> logger, CommandanteContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<Project>> GetRegisteredProjects()
        {
            if (HttpContext.User.HasClaim(c => c.Type == ClaimTypes.Role && c.Value == Roles.Admin))
            {
                return await _context.Projects.ToListAsync();
            }
            return await _context.Projects.Where(x => x.Owner.UserName == HttpContext.User.Identity.Name).ToListAsync();
        }
    }
}
