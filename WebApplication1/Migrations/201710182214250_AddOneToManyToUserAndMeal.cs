namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOneToManyToUserAndMeal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meals", "User_Id", c => c.Guid());
            CreateIndex("dbo.Meals", "User_Id");
            AddForeignKey("dbo.Meals", "User_Id", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Meals", "User_Id", "dbo.User");
            DropIndex("dbo.Meals", new[] { "User_Id" });
            DropColumn("dbo.Meals", "User_Id");
        }
    }
}
