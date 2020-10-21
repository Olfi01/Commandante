using Commandante.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Commandante.Data
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }
        public string OwnerId { get; set; }
        public CommandanteUser Owner { get; set; }
        public string Name { get; set; }
        public List<Instance> Instances { get; set; }
    }
}
