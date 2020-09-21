namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCutoffScoreToTestScoreTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TestScores", "CutoffScore", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TestScores", "CutoffScore", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
