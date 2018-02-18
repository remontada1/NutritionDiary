using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Infrastructure;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class UserMealsViewModel
    {
        public string Name { get; set; }
        public List<MealViewModel> Meals { get; set; }
        public MealTotalNutrients Total { get; set; }
    }
}