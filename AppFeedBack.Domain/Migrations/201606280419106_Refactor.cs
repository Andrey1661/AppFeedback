namespace AppFeedBack.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Refactor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        FilePath = c.String(),
                        Text = c.String(nullable: false),
                        PostDate = c.DateTime(nullable: false),
                        CategoryId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Feedbacks", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Feedbacks", new[] { "CategoryId" });
            DropTable("dbo.Feedbacks");
            DropTable("dbo.Categories");
        }
    }
}
