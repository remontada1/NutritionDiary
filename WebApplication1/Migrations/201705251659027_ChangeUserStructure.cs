namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUserStructure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "Email", c => c.String(nullable: false));
            AddColumn("dbo.Customers", "Password", c => c.String());
            AddColumn("dbo.Customers", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "CreateDate");
            DropColumn("dbo.Customers", "Password");
            DropColumn("dbo.Customers", "Email");
        }
    }
}
