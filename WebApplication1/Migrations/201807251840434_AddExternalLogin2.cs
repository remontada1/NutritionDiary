namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddExternalLogin2 : DbMigration
    {
        public override void Up()

        {
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
        }
        
        public override void Down()
        {
        }
    }
}
