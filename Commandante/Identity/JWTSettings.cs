using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commandante.Identity
{
    public class JWTSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
