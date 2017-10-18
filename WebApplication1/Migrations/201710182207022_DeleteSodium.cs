namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteSodium : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Foods", "Sodium");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Foods", "Sodium", c => c.Int(nullable: false));
        }
    }
}
