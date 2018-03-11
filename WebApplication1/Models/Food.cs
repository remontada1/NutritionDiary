using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Food
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Protein { get; set; }

        public int Hydrates { get; set; }
        public int Fats { get; set; }

        public int KCalory { get; set; }

        public string Image { get; set; }

        public int Weight { get; set; }
        
        public virtual ICollection<Meal> Meals { get; set; }

        public Food()
        {
            Meals = new List<Meal>();
        }

    }

}