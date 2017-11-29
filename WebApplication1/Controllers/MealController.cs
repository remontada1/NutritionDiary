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

        // add food to existing meal
        [HttpPost]
        [Route("meal/{mealId}/food/{foodId}")]
        public IHttpActionResult AttachMealToFood(Meal meal, int mealId, int foodId)
        {
            mealService.AddFoodToMeal(mealId, foodId);
            mealService.SaveMeals();

            return Content(HttpStatusCode.OK, "Food  have been attached to meal.");
        }

        [HttpGet]
        [Authorize]
        [Route("api/meals")]
        public IHttpActionResult GetCurrentUserMeals()
        {
            var mealList = mealService.GetCurrentUserMeal();

            return Content(HttpStatusCode.OK, mealList);
        }
        // returns meal with foods
        [HttpGet]
        [Route("meal/{mealId}/foods")]
        public IHttpActionResult GetMealAndFoodsUnauthorized(int mealId)
        {
            IEnumerable<MealViewModel> mealVm;

            var totalCalories = mealService.SumOfNutrients(mealId);
            var meals = mealService.GetMealWithFoods(mealId);
            mealVm = mapper.Map<IEnumerable<Meal>, IEnumerable<MealViewModel>>(meals);

            return Content(HttpStatusCode.OK, new { totalCalories, meals });
        }

        [HttpPost]
        [Authorize]
        [Route("api/meal")]
        public IHttpActionResult CreateMeal(Meal meal)
        {
            mealService.CreateMeal(meal);
            mealService.SaveMeals();

            return Content(HttpStatusCode.Accepted, "Meal created");
        }

        // delete existing food from meal
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