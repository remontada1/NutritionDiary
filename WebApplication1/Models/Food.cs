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
        [StringLength(50, ErrorMessage ="{0} must be at least {2} charachters long.",MinimumLength = 2)]
        public string Name { get; set; }

        [Range(0,10000,ErrorMessage ="{0} should be between {1} and {2}.")]
        public int Protein { get; set; }

        [Range(0, 10000, ErrorMessage = "{0} should be between {1} and {2}.")]
        public int Hydrates { get; set; }

        [Range(0, 10000, ErrorMessage = "{0} should be between {1} and {2}.")]
        public int Fats { get; set; }

        [Range(0, 10000, ErrorMessage = "{0} should be between {1} and {2}.")]
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