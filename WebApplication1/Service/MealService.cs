using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Infrastructure;
using WebApplication1.Models;
using WebApplication1.DAL;
using WebApplication1.Repository;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace WebApplication1.Service
{
    public class MealService : IMealService
    {
        IFoodRepository foodRepository;
        IMealRepository mealRepository;
        IUnitOfWork unitOfWork;
        IUserRepository userRepository;

        public MealService(IFoodRepository foodRepository, IMealRepository mealRepository,
            IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.foodRepository = foodRepository;
            this.mealRepository = mealRepository;
            this.userRepository = userRepository;
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

        public void UpdateMeal(Meal meal)
        {
            mealRepository.Update(meal);
        }

        public MealTotalNutrients SumOfNutrientsPerDay(DateTime date)
        {
            return mealRepository.SumOfNutrientsPerDay(date);
        }

        public MealTotalNutrients SumOfNutrientsPerMeal(int mealId)
        {
            return mealRepository.SumOfNutrientsPerMeal(mealId);
        }

        public void CreateMeal(Meal meal)
        {
            mealRepository.CreateMeal(meal);
        }

        public IEnumerable<Meal> GetCurrentUserMeal()
        {
             return mealRepository.GetCurrentUserMeals();
        }

        public void SaveMeals()
        {
            unitOfWork.Commit();
        }
    }

    public interface IMealService
    {
        void RemoveFoodFromMeal(int mealId, int foodId);
        void AddFoodToMeal(int mealId, int foodId);
        void SaveMeals();
        void UpdateMeal(Meal meal);
        Meal GetMealById(int id);
        IEnumerable<Meal> GetMeals();
        IEnumerable<Meal> GetMealWithFoods(int mealId);
        MealTotalNutrients SumOfNutrientsPerMeal(int mealId);
        MealTotalNutrients SumOfNutrientsPerDay(DateTime date);
        void CreateMeal(Meal meal);
        IEnumerable<Meal> GetCurrentUserMeal();

    }
}