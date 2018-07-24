namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JoinDateFix2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.User", "JoinDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "JoinDate", c => c.DateTime(nullable: false));
        }
    }
}
