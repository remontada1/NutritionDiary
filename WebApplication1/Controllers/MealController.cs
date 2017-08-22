﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using WebApplication1.Models;
using WebApplication1.Infrastructure;
using AttributeRouting.Web.Http;
using WebApplication1.Service;
using WebApplication1.Mappings;
using WebApplication1.ViewModels;
using AutoMapper.QueryableExtensions;
using AutoMapper;

namespace WebApplication1.Controllers
{
    public class MealController : ApiController
    {

        IMealService mealService;
        IMapper mapper;

        public MealController(IMapper mapper, IMealService mealService)
        {
            this.mealService = mealService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("meal/{mealId}/food/{foodId}")]
        public IHttpActionResult AttachMealToFood(Meal meal, int mealId, int foodId)
        {
            mealService.AddFoodToMeal(mealId, foodId);
            mealService.SaveMeals();

            return Content(HttpStatusCode.OK, "Food  have been attached to food.");
        }

        [HttpGet]
        [Route("meal/{mealId}/foods")]
        public IHttpActionResult GetMealAndFoods(int mealId)
        {
            IEnumerable<MealViewModel> mealVm;
            IEnumerable<FoodViewModel> foodVm;
            
            
            //var sum = mealService.SumOfCalories(mealId);
            var meals = mealService.GetMealWithFoods(mealId);
            mealVm = mapper.Map<IEnumerable<Meal>,IEnumerable<MealViewModel>>(meals);
     
            return Content(HttpStatusCode.OK, mealVm);
        }
        [HttpPost]
        [Route("Meal")]
        public IHttpActionResult CreateMeal(Meal meal)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.NotAcceptable, "Invalid arguments.");
            }
            mealService.AddMeal(meal);
            mealService.SaveMeals();

            return Content(HttpStatusCode.OK, "Meal created.");

        }

        [HttpDelete]
        [Route("meal/{mealId}/food/{foodId}")]
        public IHttpActionResult RemoveFoodFromMeal(int mealId, int foodId)
        {
            mealService.RemoveFoodFromMeal(mealId, foodId);
            mealService.SaveMeals();

            return Content(HttpStatusCode.Accepted, "Food deleted.");
        }
    }
}