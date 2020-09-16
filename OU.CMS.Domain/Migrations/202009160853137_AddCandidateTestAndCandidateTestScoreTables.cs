namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCandidateTestAndCandidateTestScoreTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JobOpenings", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.TestScores", "TestId", "dbo.Tests");
            CreateTable(
                "dbo.CandidateTests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CandidateId = c.Guid(nullable: false),
                        TestId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Candidates", t => t.CandidateId)
                .ForeignKey("dbo.Tests", t => t.TestId)
                .Index(t => t.CandidateId)
                .Index(t => t.TestId);
            
            CreateTable(
                "dbo.CandidateTestScores",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CandidateTestId = c.Guid(nullable: false),
                        TestScoreId = c.Guid(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CandidateTests", t => t.CandidateTestId)
                .ForeignKey("dbo.TestScores", t => t.TestScoreId)
                .Index(t => t.CandidateTestId)
                .Index(t => t.TestScoreId);
            
            AddColumn("dbo.TestScores", "IsMandatory", c => c.Boolean(nullable: false));
            AddForeignKey("dbo.JobOpenings", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.TestScores", "TestId", "dbo.Tests", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestScores", "TestId", "dbo.Tests");
            DropForeignKey("dbo.JobOpenings", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CandidateTests", "TestId", "dbo.Tests");
            DropForeignKey("dbo.CandidateTestScores", "TestScoreId", "dbo.TestScores");
            DropForeignKey("dbo.CandidateTestScores", "CandidateTestId", "dbo.CandidateTests");
            DropForeignKey("dbo.CandidateTests", "CandidateId", "dbo.Candidates");
            DropIndex("dbo.CandidateTestScores", new[] { "TestScoreId" });
            DropIndex("dbo.CandidateTestScores", new[] { "CandidateTestId" });
            DropIndex("dbo.CandidateTests", new[] { "TestId" });
            DropIndex("dbo.CandidateTests", new[] { "CandidateId" });
            DropColumn("dbo.TestScores", "IsMandatory");
            DropTable("dbo.CandidateTestScores");
            DropTable("dbo.CandidateTests");
            AddForeignKey("dbo.TestScores", "TestId", "dbo.Tests", "Id", cascadeDelete: true);
            AddForeignKey("dbo.JobOpenings", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
