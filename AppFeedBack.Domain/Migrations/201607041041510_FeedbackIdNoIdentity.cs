namespace AppFeedBack.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedbackIdNoIdentity : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FeedBackFiles", "FeedbackId", "dbo.Feedbacks");
            DropPrimaryKey("dbo.Feedbacks");
            AlterColumn("dbo.Feedbacks", "Id", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.Feedbacks", "Id");
            AddForeignKey("dbo.FeedBackFiles", "FeedbackId", "dbo.Feedbacks", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FeedBackFiles", "FeedbackId", "dbo.Feedbacks");
            DropPrimaryKey("dbo.Feedbacks");
            AlterColumn("dbo.Feedbacks", "Id", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.Feedbacks", "Id");
            AddForeignKey("dbo.FeedBackFiles", "FeedbackId", "dbo.Feedbacks", "Id", cascadeDelete: true);
        }
    }
}
