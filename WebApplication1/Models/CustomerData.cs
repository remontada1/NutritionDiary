using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class CustomerData
    {
        [Key]
        [ForeignKey("Customer")]
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Weight { get; set; }

        public int Height { get; set; }
        
        public Customer Customer {get; set;}


    }
}