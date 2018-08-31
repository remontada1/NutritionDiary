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
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace WebApplication1.Controllers
{
    public class MealController : ApiController

    {
        private readonly IMealService mealService;
        private readonly IMapper mapper;

        public MealController(IMapper mapper, IMealService mealService)
        {
            this.mealService = mealService;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        [Route("meal/{mealId}/food/{foodId}")]
        public IHttpActionResult AttachMealToFood(int mealId, int foodId)
        {
            mealService.AddFoodToMeal(mealId, foodId);
            mealService.SaveMeals();

            var currentMealFoodList = mealService.GetMealWithFoods(mealId);

            return Content(HttpStatusCode.OK, currentMealFoodList);
        }

        [Authorize]
        [HttpGet]
        [Route("api/meals")]
        public IHttpActionResult GetCurrentUserMeals()
        {
            var mealList = mealService.GetCurrentUserMeal();

            var userMealsVM = mapper.Map<IEnumerable<Meal>, IEnumerable<MealViewModel>>(mealList);

            return Content(HttpStatusCode.OK, userMealsVM);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mealId"></param>
        /// <returns>meal with foods</returns>
        [Authorize]
        [HttpGet]
        [Route("meal/{mealId}/foods")]
        public IHttpActionResult GetMealAndFoods(int mealId)
        {
            IEnumerable<MealViewModel> mealVm;

            var totalCalories = mealService.SumOfNutrientsPerMeal(mealId);

            var meals = mealService.GetMealWithFoods(mealId);

            if (totalCalories == null || meals == null)
            {
                return Content(HttpStatusCode.NotFound, "Meal not found.");
            }

            mealVm = mapper.Map<IEnumerable<Meal>, IEnumerable<MealViewModel>>(meals);

            return Content(HttpStatusCode.OK, new { totalCalories, mealVm });
        }

        [Authorize]
        [HttpPost]
        [Route("meal/foods")]
        public IHttpActionResult GetMealAndFoodsPerDay(DateDTO date)
        {
            IEnumerable<MealViewModel> mealVm;
            //string dateDay = date.Date.ToString("dd'/'MM'/'yyyy");
            var totalCalories = mealService.SumOfNutrientsPerDay(date.Date);

            var mealAndFoods = mealService.GetMealAndFoodsPerDay(date.Date);

            if (totalCalories == null || mealAndFoods == null)
            {
                return Content(HttpStatusCode.NotFound, "Meals not found.");
            }

            mealVm = mapper.Map<IEnumerable<Meal>, IEnumerable<MealViewModel>>(mealAndFoods);

            return Content(HttpStatusCode.OK, new { totalCalories, mealVm });
        }

        [Authorize]
        [HttpPost]
        [Route("api/meal")]
        public IHttpActionResult CreateMeal(Meal meal)
        {
            mealService.CreateMeal(meal);
            mealService.SaveMeals();

            return Content(HttpStatusCode.Accepted, "Meal created");
        }

        // delete existing food from meal
        [Authorize]
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