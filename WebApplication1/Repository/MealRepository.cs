using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using WebApplication1.Infrastructure;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using WebApplication1.Repository;
using System.Web;
using System.Threading.Tasks;

namespace WebApplication1.Repository
{
    public class MealRepository : RepositoryBase<Meal>, IMealRepository
    {
        private readonly IGuidRepository guidRepository;

        public MealRepository(IDbFactory dbFactory, IGuidRepository guidRepository)
            : base(dbFactory)
        {
            this.guidRepository = guidRepository;
        }

        public Meal GetMealByName(string name)
        {
            var meal = this.DbContext.Meals.Where(m => m.Name == name).FirstOrDefault();
            return meal;
        }

        public void AttachFoodToMeal(int mealId, int foodId)
        {
            var user = guidRepository.GetUserByGuid();

            var meal = this.DbContext.Meals.Find(mealId);

            if (user.Id == meal.UserId)
            {
                this.DbContext.Meals.Attach(meal);

                var food = this.DbContext.Foods.Find(foodId);
                this.DbContext.Foods.Attach(food);

                meal.Foods.Add(food);
            }
        }

        // counting total nutrients
        public MealTotalNutrients SumOfNutrients(int mealId)
        {

            var meal = this.DbContext.Meals
                .Where(m => m.Id == mealId)
                .GroupBy(m => m.Name)
                .Select(f => new MealTotalNutrients
                {
                    TotalCalories = f.Sum(k => k.Foods.Sum(c => (int?)c.KCalory)),
                    TotalFats = f.Sum(k => k.Foods.Sum(c => (int?)c.Fats)),
                    TotalCarbs = f.Sum(k => k.Foods.Sum(c => (int?)c.Hydrates)),
                    TotalProteins = f.Sum(k => k.Foods.Sum(c => (int?)c.Protein))
                }).FirstOrDefault();

            return meal;
        }

        public IEnumerable<Meal> GetMealWithFoods(int mealId)
        {
            var mealWithFoods = this.DbContext.Meals
                 .Where(m => m.Id == mealId)
                 .Include(f => f.Foods);

            return mealWithFoods;
        }

        public void RemoveFoodFromMeal(int mealId, int foodId)
        {
            var meal = this.DbContext.Meals.Find(mealId);
            var food = this.DbContext.Foods.Find(foodId);

            var user = guidRepository.GetUserByGuid();

            if (meal == null || food == null)
            {
                throw new Exception("Meal or Food not found.");
            }

            if (meal.UserId == user.Id)
            {
                this.DbContext.Entry(meal).Collection("Foods").Load();

                meal.Foods.Remove(food);
            }
            else
            {
                throw new Exception("You can't remove another's food");
            }

        }

        public void CreateMeal(Meal meal)
        {
            var user = guidRepository.GetUserByGuid();

            if (user == null)
            {
                throw new Exception("Please login to system for creating new meal.");
            }

            var mealEntity = this.DbContext.Meals.Add(meal);
            DbContext.Meals.Add(mealEntity);
            DbContext.Users.Attach(user);

            user.Meals.Add(meal);
        }

        public IEnumerable<Meal> GetCurrentUserMeals()
        {
            var user = guidRepository.GetUserByGuid();

            IEnumerable<Meal> meals = DbContext.Meals
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(d => d.SetDate);

            return meals;
        }
    }

    public interface IMealRepository : IRepository<Meal>
    {
        void RemoveFoodFromMeal(int mealId, int foodId);
        Meal GetMealByName(string name);
        void AttachFoodToMeal(int mealId, int foodId);
        IEnumerable<Meal> GetMealWithFoods(int mealId);
        MealTotalNutrients SumOfNutrients(int mealId);
        void CreateMeal(Meal meal);
        IEnumerable<Meal> GetCurrentUserMeals();
    }
}