using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class MealViewModel
    {
        public string MealName { get; set; }
        public string FoodName { get; set; }

        public int TotalCalories { get; set; }

        public int KCalories { get; set; }

        public int Weight { get; set; }

    }
}