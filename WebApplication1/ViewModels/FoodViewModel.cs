using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using AutoMapper;

namespace WebApplication1.ViewModels
{
    public class FoodViewModel
    {
        public int FoodId { get; set; }
        public string FoodName { get; set; }
        public int KCalory { get; set;}
        public int Protein { get; set;}
        public int Hydrates { get; set;}
        public int Fats {get; set;}
        public string Image { get; set; }
    }
}