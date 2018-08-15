using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.ViewModels
{
    public class MealViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MealDate { get; set; }
        public List<FoodViewModel> Foods { get; set; }
    }
}