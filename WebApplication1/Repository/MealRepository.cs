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

    }

    public interface IMealRepository : IRepository<Meal>
    {

    }
}