using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Infrastructure;
using WebApplication1.Models;
using WebApplication1.DAL;
using WebApplication1.Repository;


namespace WebApplication1.Service
{

    public interface IMealService
    {
        IEnumerable<Meal> GetMeals();
        Meal GetMealById(int id);
        Meal GetMealByName(string name);
        void AddMeal(Meal meal);

        void AddFoodToMeak(int mealId, int foodId);
        void UpdateMeal(Meal food);
        void RemoveMeal(int id);
        void SaveMeal();
    }



    public class MealService : IMealService
    {
        IFoodRepository foodRepository;
        IMealRepository mealRepository;
        IUnitOfWork unitOfWork;

        public MealService(IFoodRepository foodRepository, IMealRepository mealRepository, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.foodRepository = foodRepository;
            this.mealRepository = mealRepository;
        }

        public void AddFoodToMeal(int mealId, int foodId)
        {
            var food = foodRepository.GetById(foodId);
            var meal = mealRepository.GetById(mealId);

            
        }
    }
}