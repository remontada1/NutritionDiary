using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Service;
using WebApplication1.ViewModels;
using AutoMapper;
using System.Web.Http.Cors;
using System.Web;
using System.Net.Http;
using WebApplication1.Infrastructure;
using System.Threading.Tasks;
using System.IO;

namespace WebApplication1.Controllers
{
    
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FoodController : ApiController
    {
        private readonly IFoodService foodService;
        private readonly IMapper mapper;

        public FoodController(IFoodService foodService, IMapper mapper)
        {
            this.foodService = foodService;
            this.mapper = mapper;
        }


        [HttpGet]
        [Route("api/foods")]
        public async Task<IHttpActionResult> GetFoods()
        {
            IEnumerable<FoodViewModel> viewModelFoods;
            IEnumerable<Food> foods;
            
            foods = await foodService.GetFoodsAsync();
            if (foods == null)
            {
                return Content(HttpStatusCode.NotFound, "Foods not found");
            }
            viewModelFoods = mapper.Map<IEnumerable<Food>, IEnumerable<FoodViewModel>>(foods);
            
            return Content(HttpStatusCode.OK, viewModelFoods);
            
        }

        [HttpGet]
        [Route("api/food/{id:int}")]
        public IHttpActionResult GetFoodById(int id)
        {
            FoodViewModel viewModelFood;

            Food food = foodService.GetFoodById(id);
            if (food == null)
            {
               return Content(HttpStatusCode.NotFound, "Food not found");
            }
            viewModelFood = mapper.Map<Food, FoodViewModel>(food);

            return Content(HttpStatusCode.OK, viewModelFood);
        }

        [HttpGet]
        [Route("api/food/{name}")]
        public IHttpActionResult GetFoodByName(string name)
        {
            FoodViewModel viewModelFood;

            Food food = foodService.GetFoodByName(name);
            if (food == null)
            {
                return Content(HttpStatusCode.NotFound, "Food not found");
            }
            viewModelFood = mapper.Map<Food, FoodViewModel>(food);

            return Content(HttpStatusCode.OK, viewModelFood);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        [Route("api/food")]
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
        [Route("api/food")]
        public IHttpActionResult UpdateFood(Food food)
        {
            FoodViewModel viewModelFood;

            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, ModelState);
            }

            else
            {

                foodService.UpdateFood(food);
                foodService.SaveFood();
                viewModelFood = mapper.Map<Food, FoodViewModel>(food);

                return Content(HttpStatusCode.OK, viewModelFood);
            }
        }

        
        [HttpPost]
        [Route("api/food/upload/{foodId}")]
        public async Task<HttpResponseMessage> PostFoodImage(int foodId)
        {
            FoodViewModel viewModelFood;
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                var postedFile = httpRequest.Files[0];
                if (postedFile != null && postedFile.ContentLength > 0)
                {

                    int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png", ".jpeg" };
                    var extension = postedFile.FileName.Substring(postedFile.FileName.ToLower().LastIndexOf('.'));

                    if (!AllowedFileExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {

                        var messageImageType = string.Format("Please Upload image of type .jpg,.gif,.png.");

                        dict.Add("error", messageImageType);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                    else if (postedFile.ContentLength > MaxContentLength)
                    {

                        var messageSizeRestriction = string.Format("Please Upload a file upto 1 mb.");

                        dict.Add("error", messageSizeRestriction);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                    }
                    else
                    {
                        Food foodOld = foodService.GetFoodById(foodId);

                        var filePath = HttpContext.Current.Server.MapPath("~/Images/" + postedFile.FileName + extension);

                        postedFile.SaveAs(filePath);

                        FileUploadResult uploadResult = new FileUploadResult
                        {
                            FileLength = postedFile.ContentLength,
                            FileName = postedFile.FileName,
                            LocalFilePath = filePath
                        };

                        foodOld.Image = uploadResult.FileName;
                        foodService.UpdateFood(foodOld);
                        await foodService.SaveFoodAsync();

                        viewModelFood = mapper.Map<Food, FoodViewModel>(foodOld);

                    }

                    var successUploadMessage = string.Format("Image Updated Successfully.");
                    return Request.CreateResponse(HttpStatusCode.Created, viewModelFood); ;
                }
                var uploadImageMessage = string.Format("Please Upload an image.");
                dict.Add("error", uploadImageMessage);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("api/food/{id}")]
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