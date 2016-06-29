namespace AppFeedBack.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedbackFilePathPropRemove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Feedbacks", "FilePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Feedbacks", "FilePath", c => c.String());
        }
    }
}
