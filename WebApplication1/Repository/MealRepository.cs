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
            var meal = this.DbContext.Meals.Find(mealId);
            this.DbContext.Meals.Attach(meal);

            var food = this.DbContext.Foods.Find(foodId);
            this.DbContext.Foods.Attach(food);

            meal.Foods.Add(food);
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

        public void CreateMeal(Meal meal)
        {
            var user = guidRepository.GetUserByGuid();

            var mealEntity = this.DbContext.Meals.Add(meal);

            DbContext.Meals.Add(mealEntity);
            DbContext.Users.Attach(user);

            user.Meals.Add(meal);
        }

        public IEnumerable<Meal> GetCurrentUserMeals()
        {
            Guid guid = Guid.Empty;
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();
            guid = new Guid(currentUserId);

            var user = DbContext.Users.FirstOrDefault(i => i.Id == guid);

            IEnumerable<Meal> meals = DbContext.Meals
                .Where(x => x.UserId == guid)
                .OrderByDescending(d => d.SetDate).ToList();

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