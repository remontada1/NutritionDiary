using System;
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
        [Route("AttachMealToFood/{mealId}/{foodId}")]
        public IHttpActionResult AttachMealToFood(Meal meal, int mealId, int foodId)
        {
            mealService.AddFoodToMeal(mealId, foodId);
            mealService.SaveMeals();

            return Content(HttpStatusCode.OK, "Food  have been attached to food.");
        }

        [HttpGet]
        [Route("GetMealAndFoods/{mealId}")]
        public IHttpActionResult GetMealAndFoods(int mealId)
        {
            var sum = mealService.SumOfCalories(mealId);
            var mealWithFoods = mealService.GetMealWithFoods(mealId);
           
            return Content(HttpStatusCode.OK, sum);
        }
        [HttpPost]
        [Route("CreateMeal")]
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
        [Route("RemoveFoodFromMeal/{mealId}/{foodId}")]
        public IHttpActionResult RemoveFoodFromMeal(int mealId, int foodId)
        {
                mealService.RemoveFoodFromMeal(mealId, foodId);
                mealService.SaveMeals();

                return Content(HttpStatusCode.Accepted, "Food deleted.");
        }
    }
}