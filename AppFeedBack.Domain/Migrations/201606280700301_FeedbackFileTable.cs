namespace AppFeedBack.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedbackFileTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FeedBackFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        FilePath = c.String(nullable: false),
                        FeedbackId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Feedbacks", t => t.FeedbackId, cascadeDelete: true)
                .Index(t => t.FeedbackId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeedBackFiles", "FeedbackId", "dbo.Feedbacks");
            DropIndex("dbo.FeedBackFiles", new[] { "FeedbackId" });
            DropTable("dbo.FeedBackFiles");
        }
    }
}
