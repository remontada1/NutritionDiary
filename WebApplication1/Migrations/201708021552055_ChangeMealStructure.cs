namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMealStructure : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Meals", "MealTypeId", "dbo.MealTypes");
            DropIndex("dbo.Meals", new[] { "MealTypeId" });
            RenameColumn(table: "dbo.Meals", name: "MealTypeId", newName: "MealType_Id");
            AddColumn("dbo.Meals", "Name", c => c.String());
            AlterColumn("dbo.Meals", "MealType_Id", c => c.Int());
            CreateIndex("dbo.Meals", "MealType_Id");
            AddForeignKey("dbo.Meals", "MealType_Id", "dbo.MealTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Meals", "MealType_Id", "dbo.MealTypes");
            DropIndex("dbo.Meals", new[] { "MealType_Id" });
            AlterColumn("dbo.Meals", "MealType_Id", c => c.Int(nullable: false));
            DropColumn("dbo.Meals", "Name");
            RenameColumn(table: "dbo.Meals", name: "MealType_Id", newName: "MealTypeId");
            CreateIndex("dbo.Meals", "MealTypeId");
            AddForeignKey("dbo.Meals", "MealTypeId", "dbo.MealTypes", "Id", cascadeDelete: true);
        }
    }
}
