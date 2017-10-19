namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationOneToMany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Meals", "UserId", c => c.Guid());
            CreateIndex("dbo.Meals", "UserId");
            AddForeignKey("dbo.Meals", "UserId", "dbo.User", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Meals", "UserId", "dbo.User");
            DropIndex("dbo.Meals", new[] { "UserId" });
            DropColumn("dbo.Meals", "UserId");
        }
    }
}
