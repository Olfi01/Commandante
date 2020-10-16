using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Commandante.Data
{
    public class Instance
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Configuration { get; set; }
        public bool Running { get; set; } = false;
        public int? PID { get; set; }
        [Required]
        public string ExecutableFilePath { get; set; }
        public string Arguments { get; set; } = "";

        [Required]
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
