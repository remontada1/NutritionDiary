using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Infrastructure;


namespace WebApplication1.Repository
{
    public class MealRepository : RepositoryBase<Meal>, IMealRepository 
    {
        public MealRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Meal AttachFoodToMeal(int id)
        {
            var meal = this.DbContext.Meals.Where(m => m.Id == id).FirstOrDefault();

            return meal;


        }
        
    }

    public interface IMealRepository : IRepository<Meal>
    {

    }
}