using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication1.DAL;
using WebApplication1.Models;
using System.Data.Entity;
using WebApplication1.Infrastructure;

namespace WebApplication1.Infrastructure
{
    public class FoodRepository : RepositoryBase<Food>, IFoodRepository
    {

        public FoodRepository(IDbFactory dbFactory)
            : base(dbFactory) { }
        
        public Food GetFoodByName(string foodName)
        {
            var food = this.DbContext.Foods.Where(f => f.Name == foodName).FirstOrDefault();
            return food;
        }



    }

    public interface IFoodRepository : IRepository<Food>
    {
        Food GetFoodByName(string foodname);
    }
}
