namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFoods : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Foods", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Foods", "Image");
        }
    }
}
