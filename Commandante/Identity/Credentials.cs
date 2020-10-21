using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Commandante.Identity
{
    public class Credentials
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Key]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
