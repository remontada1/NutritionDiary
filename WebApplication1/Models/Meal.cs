using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Meal
    {
        public int Id { get; set; }

        public int MealTypeId { get; set; }
        public MealType MealType { get; set; }

        public int DayId { get; set; }

        public Day Day { get; set; }

        public int WeekNumber { get; set; }
        public int Year {get; set; }
        
    }
}