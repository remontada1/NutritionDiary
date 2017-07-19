﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Infrastructure;
using AttributeRouting.Web.Http;
using WebApplication1.Service;
using WebApplication1.Mappings;
using WebApplication1.ViewModels;
using AutoMapper;

namespace WebApplication1.Controllers
{
    [RoutePrefix("api/Food")]
    public class FoodController : ApiController
    {
        private readonly IFoodService foodService;

        public FoodController(IFoodService foodService)
        {
            this.foodService = foodService;
        }

        public HttpResponseMessage GetFoods()
        {
            IEnumerable<FoodViewModel> viewModelFoods;
            IEnumerable<Food> foods;

            foods = foodService.GetFoods().OrderByDescending(f => f.KCalory).Take(10).ToList();

            viewModelFoods = Mapper.Map<IEnumerable<Food>, IEnumerable<FoodViewModel>>(foods);

            return Request.CreateResponse(HttpStatusCode.OK, viewModelFoods);
        }
    }
}