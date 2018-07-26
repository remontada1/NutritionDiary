using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class RoleCreateBindingModel
    {

        [Required]
        [StringLength(256, ErrorMessage ="The {0} must be at least {1} charachters long.", MinimumLength =2)]
        public string Name { get; set; }
    }
}