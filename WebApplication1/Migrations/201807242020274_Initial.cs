namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.CustomerDatas",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            CreateDate = c.DateTime(nullable: false),
            //            FirstName = c.String(),
            //            LastName = c.String(),
            //            Weight = c.Int(nullable: false),
            //            Height = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.Foods",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 50),
            //            Protein = c.Int(nullable: false),
            //            Hydrates = c.Int(nullable: false),
            //            Fats = c.Int(nullable: false),
            //            KCalory = c.Int(nullable: false),
            //            Image = c.String(),
            //            Weight = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.Meals",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(),
            //            SetDate = c.DateTime(nullable: false),
            //            UserId = c.Guid(),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.User", t => t.UserId)
            //    .Index(t => t.UserId);

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

            //CreateTable(
            //    "dbo.User",
            //    c => new
            //        {
            //            Id = c.Guid(nullable: false),
            //            UserName = c.String(nullable: false, maxLength: 256),
            //            PasswordHash = c.String(maxLength: 4000),
            //            SecurityStamp = c.String(maxLength: 4000),
            //            JoinDate = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);

            //CreateTable(
            //    "dbo.MealFoods",
            //    c => new
            //        {
            //            Meal_Id = c.Int(nullable: false),
            //            Food_Id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.Meal_Id, t.Food_Id })
            //    .ForeignKey("dbo.Meals", t => t.Meal_Id, cascadeDelete: true)
            //    .ForeignKey("dbo.Foods", t => t.Food_Id, cascadeDelete: true)
            //    .Index(t => t.Meal_Id)
            //    .Index(t => t.Food_Id);

        }

        public override void Down()
        {
            //DropForeignKey("dbo.ExternalLogin", "UserId", "dbo.User");
            //DropForeignKey("dbo.Meals", "UserId", "dbo.User");
            //DropForeignKey("dbo.MealFoods", "Food_Id", "dbo.Foods");
            //DropForeignKey("dbo.MealFoods", "Meal_Id", "dbo.Meals");
            //DropIndex("dbo.MealFoods", new[] { "Food_Id" });
            //DropIndex("dbo.MealFoods", new[] { "Meal_Id" });
            //DropIndex("dbo.ExternalLogin", new[] { "UserId" });
            //DropIndex("dbo.Meals", new[] { "UserId" });
            //DropTable("dbo.MealFoods");
            //DropTable("dbo.User");
            //DropTable("dbo.ExternalLogin");
            //DropTable("dbo.Meals");
            //DropTable("dbo.Foods");
            //DropTable("dbo.CustomerDatas");
        }
    }
}
