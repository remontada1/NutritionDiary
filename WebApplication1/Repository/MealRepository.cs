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
using System.Web.Http;

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
            else
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
        }

        // counting total nutrients
        public MealTotalNutrients SumOfNutrientsPerMeal(int mealId)
        {
            var user = guidRepository.GetUserByGuid();

            var meal = this.DbContext.Meals
                .Where(m => m.Id == mealId && m.UserId == user.Id)
                .GroupBy(m => m.SetDate.Day)
                .Select(f => new MealTotalNutrients
                {
                    TotalCalories = f.Sum(k => k.Foods.Sum(c => (int?)c.KCalory)),
                    TotalFats = f.Sum(k => k.Foods.Sum(c => (int?)c.Fats)),
                    TotalCarbs = f.Sum(k => k.Foods.Sum(c => (int?)c.Hydrates)),
                    TotalProteins = f.Sum(k => k.Foods.Sum(c => (int?)c.Protein))
                }).FirstOrDefault();

            return meal;
        }

        public MealTotalNutrients SumOfNutrientsPerDay(DateTime date)
        {

            var user = guidRepository.GetUserByGuid();

            var meal = this.DbContext.Meals
                .Where( d => DbFunctions.TruncateTime(d.SetDate) == DbFunctions.TruncateTime(date))
                .GroupBy(g => g.SetDate.Day)
                .Select(m => new MealTotalNutrients
                {
                    TotalCalories = m.Sum(f => f.Foods.Sum(c => (int?)c.KCalory)),
                    TotalFats = m.Sum(k => k.Foods.Sum(c => (int?)c.Fats)),
                    TotalCarbs = m.Sum(k => k.Foods.Sum(c => (int?)c.Hydrates)),
                    TotalProteins = m.Sum(k => k.Foods.Sum(c => (int?)c.Protein))
                }).FirstOrDefault();

            return meal;
        }
        public IEnumerable<Meal> GetMealWithFoods(int mealId)
        {

            User user = guidRepository.GetUserByGuid();
            var mealWithFoods = this.DbContext.Meals
                 .Where(m => m.Id == mealId && m.UserId == user.Id)
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

        public IEnumerable<Meal> GetMealAndFoodsPerDay(DateTime day)
        {

            var mealAndFood = this.DbContext.Meals
                .Where(x => x.SetDate.Date == day.Date)
                .Include(f => f.Foods);

            return mealAndFood;
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
                .OrderBy(d => d.SetDate);

            return meals;
        }
    }

    public interface IMealRepository : IRepository<Meal>
    {
        void RemoveFoodFromMeal(int mealId, int foodId);
        Meal GetMealByName(string name);
        void AttachFoodToMeal(int mealId, int foodId);
        IEnumerable<Meal> GetMealWithFoods(int mealId);
        MealTotalNutrients SumOfNutrientsPerMeal(int mealId);
        void CreateMeal(Meal meal);
        IEnumerable<Meal> GetCurrentUserMeals();
        MealTotalNutrients SumOfNutrientsPerDay(DateTime date);
    }
}