namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSodiumToFood : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Foods", "Sodium", c => c.Int(nullable: false));
            DropForeignKey("dbo.MealTypes", "MealTypeId", "dbo.Meals");
            DropColumn("dbo.Meals", "MealType_Id"); 
        }
        
        public override void Down()
        {
            DropColumn("dbo.Foods", "Sodium");
        }
    }
}
