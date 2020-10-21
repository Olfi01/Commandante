using Commandante.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commandante.Identity
{
    public class CommandanteUser : IdentityUser
    {
        public CommandanteUser(string userName) : base(userName)
        {
        }

        public List<Project> Projects { get; set; }
    }
}
