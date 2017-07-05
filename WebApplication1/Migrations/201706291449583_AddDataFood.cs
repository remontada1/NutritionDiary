namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataFood : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "Password", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Customers", "ConfirmPassword", c => c.String(nullable: false, maxLength: 30));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "ConfirmPassword", c => c.String());
            AlterColumn("dbo.Customers", "Password", c => c.String());
        }
    }
}
