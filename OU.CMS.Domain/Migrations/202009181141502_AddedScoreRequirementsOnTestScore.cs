namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedScoreRequirementsOnTestScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestScores", "MinimumScore", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TestScores", "MaximumScore", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.TestScores", "CutoffScore", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.CandidateTestScores", "Value", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CandidateTestScores", "Value", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.TestScores", "CutoffScore");
            DropColumn("dbo.TestScores", "MaximumScore");
            DropColumn("dbo.TestScores", "MinimumScore");
        }
    }
}
