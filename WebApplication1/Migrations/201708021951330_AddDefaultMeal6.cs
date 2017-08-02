namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultMeal6 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FoodMeals", newName: "MealFoods");
            DropForeignKey("dbo.Meals", "Id", "dbo.Customers");
            DropIndex("dbo.Meals", new[] { "Id" });
            DropPrimaryKey("dbo.MealFoods");
            AddPrimaryKey("dbo.MealFoods", new[] { "Meal_Id", "Food_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.MealFoods");
            AddPrimaryKey("dbo.MealFoods", new[] { "Food_Id", "Meal_Id" });
            CreateIndex("dbo.Meals", "Id");
            AddForeignKey("dbo.Meals", "Id", "dbo.Customers", "Id");
            RenameTable(name: "dbo.MealFoods", newName: "FoodMeals");
        }
    }
}
