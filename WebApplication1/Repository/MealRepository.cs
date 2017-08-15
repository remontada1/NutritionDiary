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
            if (meal == null)
            {
                throw new Exception("Meal not found");
            }
            var food = this.DbContext.Foods.Find(foodId);

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
    }
}