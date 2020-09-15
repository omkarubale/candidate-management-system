namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCandidateAndJobOpeningTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CompanyManagements", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.CompanyManagements", "UserId", "dbo.Users");
            CreateTable(
                "dbo.Candidates",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        JobOpeningId = c.Guid(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .ForeignKey("dbo.JobOpenings", t => t.JobOpeningId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.CompanyId)
                .Index(t => t.JobOpeningId);
            
            CreateTable(
                "dbo.JobOpenings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        CompanyId = c.Guid(nullable: false),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            AddColumn("dbo.Companies", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Companies", "CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.CompanyManagements", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.CompanyManagements", "CreatedBy", c => c.Guid(nullable: false));
            AddColumn("dbo.Tests", "CreatedOn", c => c.DateTime(nullable: false));
            AddColumn("dbo.Tests", "CreatedBy", c => c.Guid(nullable: false));
            AddForeignKey("dbo.CompanyManagements", "CompanyId", "dbo.Companies", "Id");
            AddForeignKey("dbo.CompanyManagements", "UserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyManagements", "UserId", "dbo.Users");
            DropForeignKey("dbo.CompanyManagements", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Candidates", "UserId", "dbo.Users");
            DropForeignKey("dbo.Candidates", "JobOpeningId", "dbo.JobOpenings");
            DropForeignKey("dbo.Candidates", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.JobOpenings", "CompanyId", "dbo.Companies");
            DropIndex("dbo.JobOpenings", new[] { "CompanyId" });
            DropIndex("dbo.Candidates", new[] { "JobOpeningId" });
            DropIndex("dbo.Candidates", new[] { "CompanyId" });
            DropIndex("dbo.Candidates", new[] { "UserId" });
            DropColumn("dbo.Tests", "CreatedBy");
            DropColumn("dbo.Tests", "CreatedOn");
            DropColumn("dbo.CompanyManagements", "CreatedBy");
            DropColumn("dbo.CompanyManagements", "CreatedOn");
            DropColumn("dbo.Companies", "CreatedBy");
            DropColumn("dbo.Companies", "CreatedOn");
            DropTable("dbo.JobOpenings");
            DropTable("dbo.Candidates");
            AddForeignKey("dbo.CompanyManagements", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CompanyManagements", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
    }
}
