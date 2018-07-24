namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJoinDateAgain : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "JoinDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "JoinDate");
        }
    }
}
