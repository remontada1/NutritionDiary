namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixUserModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "CustomerData_Id", "dbo.CustomerDatas");
            DropIndex("dbo.User", new[] { "CustomerData_Id" });
            DropColumn("dbo.User", "LastName");
            DropColumn("dbo.User", "JoinDate");
            DropColumn("dbo.User", "Weight");
            DropColumn("dbo.User", "CustomerData_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "CustomerData_Id", c => c.Int());
            AddColumn("dbo.User", "Weight", c => c.Int(nullable: false));
            AddColumn("dbo.User", "JoinDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.User", "LastName", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.User", "CustomerData_Id");
            AddForeignKey("dbo.User", "CustomerData_Id", "dbo.CustomerDatas", "Id");
        }
    }
}
