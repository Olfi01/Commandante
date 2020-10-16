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
        public string Name { get; set; }
        public ProjectType Type { get; set; }
        public List<Instance> Instances { get; set; }
    }
}
