using System;
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
using NLog;
namespace WebApplication1.Controllers
{

    public class FoodController : ApiController
    {
        private readonly IFoodService foodService;
        private readonly IMapper mapper;
        
        public FoodController(IFoodService foodService, IMapper mapper)
        {
            this.foodService = foodService;
            this.mapper = mapper;
            

        }

        [Route("api/getFoods")]
        public IHttpActionResult GetFoods()
        {
            IEnumerable<FoodViewModel> viewModelFoods;
            IEnumerable<Food> foods;

            foods = foodService.GetFoods().OrderByDescending(f => f.Id).Take(5).ToList();
            if (foods == null)
            {
                return Content(HttpStatusCode.NotFound, "Foods not found");
            }
            viewModelFoods = mapper.Map<IEnumerable<Food>, IEnumerable<FoodViewModel>>(foods);

            return Content(HttpStatusCode.Found, viewModelFoods);

            throw new Exception("No product found for this id");  
        }

        public IHttpActionResult GetFoodById(int id)
        {
            FoodViewModel viewModelFood;

            Food food = foodService.GetFoodById(id);
            if (food == null)
            {
                throw new Exception("Food not found");
            }
            viewModelFood = mapper.Map<Food, FoodViewModel>(food);
            return Content(HttpStatusCode.OK, viewModelFood);

        }

        [HttpPost]
        public IHttpActionResult AddFood(Food food)
        {
            FoodViewModel viewModelFood;

            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.NotAcceptable, "Invalid arguments.");
            }
            foodService.AddFood(food);
            foodService.SaveFood();
            viewModelFood = mapper.Map<Food, FoodViewModel>(food);

            return Content(HttpStatusCode.Created, viewModelFood);
        }

        [HttpPut]
        public IHttpActionResult UpdateFood(Food food)
        {

            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, ModelState);
            }
            else
            {
                food = foodService.GetFoodById(food.Id);
                if (food == null)
                {
                    return Content(HttpStatusCode.NotFound, "Food does not exist");
                }
                else
                {
                    foodService.UpdateFood(food);
                    foodService.SaveFood();


                    return Content(HttpStatusCode.OK, food);
                }
            }
        }


        [HttpDelete]
        [Route("api/deleteFoodById/{id}")]
        public IHttpActionResult RemoveFood(int id)
        {

            Food food = foodService.GetFoodById(id);

            if (food == null)
            {
                return Content(HttpStatusCode.NotFound, "Food does not exist.");
            }
            else
            {
                foodService.Remove(id);
                foodService.SaveFood();
                return Ok("Food has been deleted.");
            }
        }
    }
}