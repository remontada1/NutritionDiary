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
        [Route("GetMealWithFoods/{mealId}/{foodId}")]
        public IHttpActionResult GetMealWithFoods(Meal meal, int mealId, int foodId)
        {
            mealService.AddFoodToMeal(mealId, foodId);
            mealService.SaveMeals();
            return Content(HttpStatusCode.OK,"Meal binding created");
        }

    }
}