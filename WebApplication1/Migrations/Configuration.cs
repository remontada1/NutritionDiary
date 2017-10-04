namespace WebApplication1.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApplication1.Models;
    using WebApplication1.DAL;


    internal sealed class Configuration : DbMigrationsConfiguration<WebApplication1.DAL.CustomerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WebApplication1.DAL.CustomerContext";
        }

        protected override void Seed(WebApplication1.DAL.CustomerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            
            
            context.Meals.AddOrUpdate(m => m.Id,
                new Meal()
                {
                    
                    Name = "Завтрак",
                    SetDate = DateTime.Parse("17/10/2016")

                });

            

           


            context.Commit();
        }
    }
}
