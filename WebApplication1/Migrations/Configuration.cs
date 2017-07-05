namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApplication1.Models;

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

            context.Foods.AddOrUpdate(
                p => p.Id,
                 new Food()
                 {
                     Id = 1,
                     Hydrates = 70,
                     KCalory = 340,
                     Fats = 3,
                     Name = "Гречка",
                     Protein = 9
                 });
        }
    }
}
