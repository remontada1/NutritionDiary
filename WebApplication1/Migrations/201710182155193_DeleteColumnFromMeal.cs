namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteColumnFromMeal : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Meals", "MealType_Id");
        }
        
        public override void Down()
        {
        }
    }
}
