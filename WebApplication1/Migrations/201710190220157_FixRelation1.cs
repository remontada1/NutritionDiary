namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixRelation1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Meals", "User_Id", "dbo.User");
            DropIndex("dbo.Meals", new[] { "User_Id" });
            DropColumn("dbo.Meals", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Meals", "User_Id", c => c.Guid());
            CreateIndex("dbo.Meals", "User_Id");
            AddForeignKey("dbo.Meals", "User_Id", "dbo.User", "Id");
        }
    }
}
