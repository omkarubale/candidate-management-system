namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedCandidateTestScore_Comment_To_Nullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CandidateTestScores", "Comment", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CandidateTestScores", "Comment", c => c.String(nullable: false, maxLength: 250));
        }
    }
}
