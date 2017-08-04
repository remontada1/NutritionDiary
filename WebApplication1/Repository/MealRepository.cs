using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Infrastructure;
using WebApplication1.DAL;
using System.Data.Entity;

namespace WebApplication1.Repository
{
    public class MealRepository : RepositoryBase<Meal>, IMealRepository
    {
        public MealRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        IFoodRepository foodRepository;



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

            this.DbContext.Foods.Add(food);

            meal.Foods.Add(food);
        }

        Meal GetMealWithFoods(int mealId)
        {
            var mealWithFoods = this.DbContext.Meals
                .Where(m => m.Id == mealId)
                .Include(f => f.Foods)
                .FirstOrDefault();

            return mealWithFoods;
        }

    }

    public interface IMealRepository : IRepository<Meal>
    {
        Meal GetMealById(string name);
        void AttachFoodToMeal(int mealId, int foodId);
        Meal GetMealWithFoods(int mealId);
    }
}