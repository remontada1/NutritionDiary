using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace WebApplication1.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Email{ get; set; }

        public string Password { get; set; }

        public CustomerData CustomerData { get; set; }
        
    }

}