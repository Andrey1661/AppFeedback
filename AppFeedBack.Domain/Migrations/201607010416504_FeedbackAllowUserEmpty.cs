namespace AppFeedBack.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedbackAllowUserEmpty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Feedbacks", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Feedbacks", "UserName", c => c.String(nullable: false));
        }
    }
}
