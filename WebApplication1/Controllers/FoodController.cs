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
        [Route("api/GetFoods")]
        public HttpResponseMessage GetFoods()
        {
            IEnumerable<FoodViewModel> viewModelFoods;
            IEnumerable<Food> foods;

            foods = foodService.GetFoods().OrderByDescending(f => f.KCalory).Take(10).ToList();

            viewModelFoods = mapper.Map<IEnumerable<Food>, IEnumerable<FoodViewModel>>(foods);

            return Request.CreateResponse(HttpStatusCode.OK, viewModelFoods);
        }

        public HttpResponseMessage GetFoodById(int id)
        {
            FoodViewModel viewModelFood;

            Food food = foodService.GetFoodById(id);

            viewModelFood = mapper.Map<Food, FoodViewModel>(food);
            return Request.CreateResponse(HttpStatusCode.OK, viewModelFood);
             
        }
        [HttpPost]
        public HttpResponseMessage AddFood( Food food)
        {

            FoodViewModel viewModelFood;
            if (ModelState.IsValid)
            {
                foodService.AddFood(food);
                foodService.SaveFood();
            }
            

            viewModelFood = mapper.Map<Food, FoodViewModel>(food);

            return  Request.CreateResponse(HttpStatusCode.Created);  
        }
        
        [HttpDelete]
        [Route("api/DeleteFoodById/{id}")]
        public  IHttpActionResult RemoveFood( int id)
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