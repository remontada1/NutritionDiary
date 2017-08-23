using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Infrastructure;
using WebApplication1.DAL;
using System.Data.Entity;
using System.Web.Http;

namespace WebApplication1.Repository
{
    public class MealRepository : RepositoryBase<Meal>, IMealRepository
    {


        public MealRepository(IDbFactory dbFactory)
            : base(dbFactory) { }


        public Meal GetMealById(string name)
        {
            var meal = this.DbContext.Meals.Where(m => m.Name == name).FirstOrDefault();
            return meal;
        }

        public void AttachFoodToMeal(int mealId, int foodId)
        {
            var meal = this.DbContext.Meals.Find(mealId);
            this.DbContext.Meals.Attach(meal);

            var food = this.DbContext.Foods.Find(foodId);
            this.DbContext.Foods.Attach(food);

            meal.Foods.Add(food);
        }

        public MealTotalValue SumOfCalories(int mealId)
        {
            var meal = this.DbContext.Meals
                .Where(m => m.Id == mealId)
                .GroupBy(m => m.Name)
                .Select(f => new MealTotalValue
                {
                    TotalCalories = f.Sum(k => k.Foods.Sum(c => (int?)c.KCalory)),
                    TotalFats = f.Sum(k => k.Foods.Sum(c => (int?)c.Fats)),
                    TotalCarbs = f.Sum(k => k.Foods.Sum(c => (int?)c.Hydrates)),
                    TotalProteins = f.Sum(k => k.Foods.Sum(c => (int?)c.Protein))
                }).FirstOrDefault();
             //   .Sum(f => f.Foods.Sum(k => (int)k.KCalory));
            return meal;
        }
        public IEnumerable<Meal> GetMealWithFoods(int mealId)
        {

            var mealWithFoods = this.DbContext.Meals
                 .Where(m => m.Id == mealId)
                 .Include(f => f.Foods)
                 .ToList();

            return mealWithFoods;
        }


        public void RemoveFoodFromMeal(int mealId, int foodId)
        {
            var meal = this.DbContext.Meals.Find(mealId);

            var food = this.DbContext.Foods.Find(foodId);
            if (meal == null || food == null)
            {
                throw new Exception("Meal or Food not found.");
            }

            this.DbContext.Entry(meal).Collection("Foods").Load();

            meal.Foods.Remove(food);


        }
    }

    public interface IMealRepository : IRepository<Meal>
    {
        void RemoveFoodFromMeal(int mealId, int foodId);
        Meal GetMealById(string name);
        void AttachFoodToMeal(int mealId, int foodId);
        IEnumerable<Meal> GetMealWithFoods(int mealId);


        MealTotalValue SumOfCalories(int mealId);
    }
}