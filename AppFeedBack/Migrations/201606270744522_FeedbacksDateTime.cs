namespace AppFeedBack.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeedbacksDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedbacks", "PostDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "PostDate");
        }
    }
}
