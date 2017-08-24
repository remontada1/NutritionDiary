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


        public void RemoveFoodFromMeal(int mealId, int foodId)
        {

            mealRepository.RemoveFoodFromMeal(mealId, foodId);

        }
        public void AddFoodToMeal(int mealId, int foodId)
        {
            mealRepository.AttachFoodToMeal(mealId, foodId);
        }

        public IEnumerable<Meal> GetMealWithFoods(int mealId)
        {
            return mealRepository.GetMealWithFoods(mealId);
        }

        public IEnumerable<Meal> GetMeals()
        {
            return mealRepository.GetAll();
        }

        public IEnumerable<Meal> SortMealsByDate()
        {
            var meals = mealRepository.GetAll();
            return meals.OrderByDescending(d => d.SetDate);
        }

        public Meal GetMealById(int id)
        {
            return mealRepository.GetById(id);
        }

        public void AddMeal(Meal meal)
        {
            mealRepository.Add(meal);
        }

        public void UpdateMeal(Meal meal)
        {
            mealRepository.Update(meal);
        }
        public MealTotalValue SumOfNutrients(int mealId)
        {
            return mealRepository.SumOfNutrients(mealId);
        }

        public void SaveMeals()
        {
            unitOfWork.Commit();
        }

        //TODO IMealService
    }

    public interface IMealService
    {
        void RemoveFoodFromMeal(int mealId, int foodId);
        void AddFoodToMeal(int mealId, int foodId);
        void SaveMeals();
        void UpdateMeal(Meal meal);
        void AddMeal(Meal meal);
        Meal GetMealById(int id);
        IEnumerable<Meal> GetMeals();
        IEnumerable<Meal> GetMealWithFoods(int mealId);
        MealTotalValue SumOfNutrients(int mealId);
    }
}