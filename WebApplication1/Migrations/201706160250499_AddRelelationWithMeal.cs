namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelelationWithMeal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Meals", "DayId", "dbo.Days");
            DropForeignKey("dbo.Meals", "Food_Id", "dbo.Foods");
            DropIndex("dbo.Meals", new[] { "DayId" });
            DropIndex("dbo.Meals", new[] { "Food_Id" });
            DropPrimaryKey("dbo.Meals");
            CreateTable(
                "dbo.FoodMeals",
                c => new
                    {
                        Food_Id = c.Int(nullable: false),
                        Meal_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Food_Id, t.Meal_Id })
                .ForeignKey("dbo.Foods", t => t.Food_Id, cascadeDelete: true)
                .ForeignKey("dbo.Meals", t => t.Meal_Id, cascadeDelete: true)
                .Index(t => t.Food_Id)
                .Index(t => t.Meal_Id);
            
            AddColumn("dbo.Meals", "SetDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Meals", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Meals", "Id");
            CreateIndex("dbo.Meals", "Id");
            AddForeignKey("dbo.Meals", "Id", "dbo.Customers", "Id");
            DropColumn("dbo.Meals", "DayId");
            DropColumn("dbo.Meals", "WeekNumber");
            DropColumn("dbo.Meals", "Year");
            DropColumn("dbo.Meals", "Food_Id");
            DropTable("dbo.Days");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Meals", "Food_Id", c => c.Int());
            AddColumn("dbo.Meals", "Year", c => c.Int(nullable: false));
            AddColumn("dbo.Meals", "WeekNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Meals", "DayId", c => c.Int(nullable: false));
            DropForeignKey("dbo.FoodMeals", "Meal_Id", "dbo.Meals");
            DropForeignKey("dbo.FoodMeals", "Food_Id", "dbo.Foods");
            DropForeignKey("dbo.Meals", "Id", "dbo.Customers");
            DropIndex("dbo.FoodMeals", new[] { "Meal_Id" });
            DropIndex("dbo.FoodMeals", new[] { "Food_Id" });
            DropIndex("dbo.Meals", new[] { "Id" });
            DropPrimaryKey("dbo.Meals");
            AlterColumn("dbo.Meals", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Meals", "SetDate");
            DropTable("dbo.FoodMeals");
            AddPrimaryKey("dbo.Meals", "Id");
            CreateIndex("dbo.Meals", "Food_Id");
            CreateIndex("dbo.Meals", "DayId");
            AddForeignKey("dbo.Meals", "Food_Id", "dbo.Foods", "Id");
            AddForeignKey("dbo.Meals", "DayId", "dbo.Days", "Id", cascadeDelete: true);
        }
    }
}
