﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.ViewModels
{
    public class UserMealsViewModel
    {
        public string Name { get; set; }
        public List<string> Meals { get; set; }
    }
}