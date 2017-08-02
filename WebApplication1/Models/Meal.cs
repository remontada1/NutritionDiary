using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Meal
    {
        [Key]
        [ForeignKey("Customer")]
        public int Id { get; set; }
        public string Name { get; set; }


        public DateTime SetDate { get; set; }
        public Customer Customer { get; set; }

        public virtual ICollection<Food> Foods { get; set; }

        public Meal()
        {
            Foods = new List<Food>();
        }

    }
}