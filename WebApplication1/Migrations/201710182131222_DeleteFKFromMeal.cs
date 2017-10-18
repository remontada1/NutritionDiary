namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteFKFromMeal : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Meals", "IX_MealType_Id");
            DropForeignKey("dbo.MealTypes", "MealTypeId", "dbo.Meals");
            DropColumn("dbo.Meals", "MealType_Id");
            
            
        }

        public override void Down()
        {
          
        }
    }
}
