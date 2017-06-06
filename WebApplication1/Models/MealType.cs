using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MealType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Meal> Meals{ get; set; }
       
        MealType()
        {
            Meals = new List<Meal>();
        }

        
    }  

}