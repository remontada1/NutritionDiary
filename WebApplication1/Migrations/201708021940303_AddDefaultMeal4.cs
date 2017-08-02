namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDefaultMeal4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FoodMeals", "Meal_Id", "dbo.Meals");
            DropIndex("dbo.Meals", new[] { "Id" });
            DropPrimaryKey("dbo.Meals");
            AlterColumn("dbo.Meals", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Meals", "Id");
            CreateIndex("dbo.Meals", "Id");
            AddForeignKey("dbo.FoodMeals", "Meal_Id", "dbo.Meals", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FoodMeals", "Meal_Id", "dbo.Meals");
            DropIndex("dbo.Meals", new[] { "Id" });
            DropPrimaryKey("dbo.Meals");
            AlterColumn("dbo.Meals", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Meals", "Id");
            CreateIndex("dbo.Meals", "Id");
            AddForeignKey("dbo.FoodMeals", "Meal_Id", "dbo.Meals", "Id", cascadeDelete: true);
        }
    }
}
