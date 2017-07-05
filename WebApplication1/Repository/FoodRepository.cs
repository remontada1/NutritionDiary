using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication1.DAL;
using WebApplication1.Models;
using System.Data.Entity;

namespace WebApplication1.Infrastructure
{
    public class FoodRepository : IRepository<Food>
    {
        
            private CustomerContext db;

            public FoodRepository(CustomerContext context)
            {
                this.db = context;
            }

            public IEnumerable<Food> GetAll()
            {
                return db.Foods;
            }

            public Food Get(int id)
            {
                return db.Foods.Find(id);
            }

            public void Create(Food food)
            {
                db.Foods.Add(food);
            }

            public void Update(Food food)
            {
                db.Entry(food).State = EntityState.Modified;
            }

            public void Remove(int id)
            {
               Food food = db.Foods.Find(id);
                if (food != null)
                    db.Foods.Remove(food);
            }
        }


    }
