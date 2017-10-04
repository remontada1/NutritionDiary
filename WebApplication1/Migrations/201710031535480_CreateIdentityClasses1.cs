namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateIdentityClasses1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CustomerDatas", "Id", "dbo.Customers");
            DropIndex("dbo.CustomerDatas", new[] { "Id" });
            DropPrimaryKey("dbo.CustomerDatas");
            CreateTable(
                "dbo.ExternalLogin",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        PasswordHash = c.String(maxLength: 4000),
                        SecurityStamp = c.String(maxLength: 4000),
                        LastName = c.String(nullable: false, maxLength: 100),
                        JoinDate = c.DateTime(nullable: false),
                        Weight = c.Int(nullable: false),
                        CustomerData_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomerDatas", t => t.CustomerData_Id)
                .Index(t => t.CustomerData_Id);
            
            AlterColumn("dbo.CustomerDatas", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.CustomerDatas", "Id");
           
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(nullable: false, maxLength: 30),
                        ConfirmPassword = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.ExternalLogin", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "CustomerData_Id", "dbo.CustomerDatas");
            DropIndex("dbo.User", new[] { "CustomerData_Id" });
            DropIndex("dbo.ExternalLogin", new[] { "UserId" });
            DropPrimaryKey("dbo.CustomerDatas");
            AlterColumn("dbo.CustomerDatas", "Id", c => c.Int(nullable: false));
            DropTable("dbo.User");
            DropTable("dbo.ExternalLogin");
            AddPrimaryKey("dbo.CustomerDatas", "Id");
            CreateIndex("dbo.CustomerDatas", "Id");
            AddForeignKey("dbo.CustomerDatas", "Id", "dbo.Customers", "Id");
        }
    }
}
