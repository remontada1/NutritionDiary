using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Infrastructure;
using WebApplication1.Models;
using WebApplication1.DAL;

namespace WebApplication1.Service
{
    public class FoodService
    {
        private readonly IFoodRepository foodRepository;
        private readonly IUnitOfWork unitOfWork;


        public FoodService(IFoodRepository foodRepository, IUnitOfWork unitOfWork)
        {
            this.foodRepository = foodRepository;
            this.unitOfWork = unitOfWork;
        }


        public IEnumerable<Food> GetFoods()
        {
            var foods = foodRepository.GetAll();
            return foods;
        }
    }
}