using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Day
    {
        public int Id { get; set; }

        public string DayName { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }
        Day()
        {
            Meals = new List<Meal>(); 
        }
    }
}