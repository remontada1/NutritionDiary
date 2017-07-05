namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableRelationsComplete2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Password = c.String(),
                        ConfirmPassword = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerDatas",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Weight = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Days",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DayName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Meals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MealTypeId = c.Int(nullable: false),
                        DayId = c.Int(nullable: false),
                        WeekNumber = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Food_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Days", t => t.DayId, cascadeDelete: true)
                .ForeignKey("dbo.MealTypes", t => t.MealTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Foods", t => t.Food_Id)
                .Index(t => t.MealTypeId)
                .Index(t => t.DayId)
                .Index(t => t.Food_Id);
            
            CreateTable(
                "dbo.MealTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Protein = c.Int(nullable: false),
                        Hydrates = c.Int(nullable: false),
                        Fats = c.Int(nullable: false),
                        KCalory = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Meals", "Food_Id", "dbo.Foods");
            DropForeignKey("dbo.Meals", "MealTypeId", "dbo.MealTypes");
            DropForeignKey("dbo.Meals", "DayId", "dbo.Days");
            DropForeignKey("dbo.CustomerDatas", "Id", "dbo.Customers");
            DropIndex("dbo.Meals", new[] { "Food_Id" });
            DropIndex("dbo.Meals", new[] { "DayId" });
            DropIndex("dbo.Meals", new[] { "MealTypeId" });
            DropIndex("dbo.CustomerDatas", new[] { "Id" });
            DropTable("dbo.Foods");
            DropTable("dbo.MealTypes");
            DropTable("dbo.Meals");
            DropTable("dbo.Days");
            DropTable("dbo.CustomerDatas");
            DropTable("dbo.Customers");
        }
    }
}
