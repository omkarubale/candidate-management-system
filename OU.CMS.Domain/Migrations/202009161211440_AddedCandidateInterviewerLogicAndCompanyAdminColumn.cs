namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCandidateInterviewerLogicAndCompanyAdminColumn : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CandidateInterviewers",
                c => new
                    {
                        CandidateId = c.Guid(nullable: false),
                        InterviewerUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.CandidateId, t.InterviewerUserId })
                .ForeignKey("dbo.Candidates", t => t.CandidateId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.InterviewerUserId, cascadeDelete: true)
                .Index(t => t.CandidateId)
                .Index(t => t.InterviewerUserId);
            
            AddColumn("dbo.CompanyManagements", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CandidateInterviewers", "InterviewerUserId", "dbo.Users");
            DropForeignKey("dbo.CandidateInterviewers", "CandidateId", "dbo.Candidates");
            DropIndex("dbo.CandidateInterviewers", new[] { "InterviewerUserId" });
            DropIndex("dbo.CandidateInterviewers", new[] { "CandidateId" });
            DropColumn("dbo.CompanyManagements", "IsAdmin");
            DropTable("dbo.CandidateInterviewers");
        }
    }
}
