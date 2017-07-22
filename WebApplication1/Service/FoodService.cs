using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Infrastructure;
using WebApplication1.Models;
using WebApplication1.DAL;

namespace WebApplication1.Service
{


    public interface IFoodService
    {
        IEnumerable<Food> GetFoods();
        Food GetFoodById(int id);
        Food GetFood(string name);

    }
    public class FoodService :IFoodService
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
        public Food GetFood(string name)
        {
            var food = foodRepository.GetFoodByName(name);
            return food;
        }

        public Food GetFoodById(int id)
        {
            var food = foodRepository.GetById(id);
            return food;
        }

        public void AddFood(Food food)
        {
            foodRepository.Add(food);
        }

        public void SaveFood()
        {
            unitOfWork.Commit();
        }
        


    }
}