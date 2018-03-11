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

        [Route("api/foods")]
        public IHttpActionResult GetFoods()
        {
            IEnumerable<FoodViewModel> viewModelFoods;
            IEnumerable<Food> foods;

            foods = foodService.GetFoods();
            if (foods == null)
            {
                return Content(HttpStatusCode.NotFound, "Foods not found");
            }
            viewModelFoods = mapper.Map<IEnumerable<Food>, IEnumerable<FoodViewModel>>(foods);

            return Content(HttpStatusCode.OK, viewModelFoods);
        }
        [HttpGet]
        [Route("api/food/id/{id}")]
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
        [Route("api/upload/{foodId}")]
        public async Task<HttpResponseMessage> PostUserImage(int foodId)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            try
            {

                var httpRequest = HttpContext.Current.Request;

                foreach (string file in httpRequest.Files)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);

                    var postedFile = httpRequest.Files[file];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {

                        int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var extension = postedFile.FileName.Substring(postedFile.FileName.ToLower().LastIndexOf('.'));

                        if (!AllowedFileExtensions.Contains(extension))
                        {

                            var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else if (postedFile.ContentLength > MaxContentLength)
                        {

                            var message = string.Format("Please Upload a file upto 1 mb.");

                            dict.Add("error", message);
                            return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                        }
                        else
                        {
                            var foodOld = foodService.GetFoodById(foodId);

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
                            await foodService.SaveFoodAsync() ;

                        }
                    }

                    var message1 = string.Format("Image Updated Successfully.");
                    return Request.CreateErrorResponse(HttpStatusCode.Created, message1); ;
                }
                var res = string.Format("Please Upload a image.");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
            catch (Exception ex)
            {
                var res = string.Format("some Message");
                dict.Add("error", res);
                return Request.CreateResponse(HttpStatusCode.NotFound, dict);
            }
        }

        //[MimeMultipart]
        //[HttpPut]
        //[Route("api/upload/{foodId}")]
        //public IHttpActionResult PostImage(int foodId)
        //{
        //    var foodOld = foodService.GetFoodById(foodId);

        //    var uploadPath = HttpContext.Current.Server.MapPath("~/Images");

        //    var streamProvider = new UploadMultipartFormProvider(uploadPath);
        //    Request.Content.ReadAsMultipartAsync(streamProvider);

        //    string _localFileName = streamProvider.FileData.Select(m => m.LocalFileName).FirstOrDefault();
        //    FileUploadResult fileUploadResult = new FileUploadResult
        //    {
        //        LocalFilePath = _localFileName,
        //        FileName = Path.GetFileName(_localFileName),
        //        FileLength = new FileInfo(_localFileName).Length
        //    };

        //    foodOld.Image = fileUploadResult.FileName;
        //    foodService.UpdateFood(foodOld);

        //    foodService.SaveFood();

        //    return Content(HttpStatusCode.OK, fileUploadResult);
        //}

        [HttpDelete]
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