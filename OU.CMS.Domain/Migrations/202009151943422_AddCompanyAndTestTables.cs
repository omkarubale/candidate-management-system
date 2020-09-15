namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyAndTestTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CompanyManagements",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.CompanyId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestScores",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        TestId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .Index(t => t.TestId);
            
            AddColumn("dbo.Users", "DateOfBirth", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestScores", "TestId", "dbo.Tests");
            DropForeignKey("dbo.CompanyManagements", "UserId", "dbo.Users");
            DropForeignKey("dbo.CompanyManagements", "CompanyId", "dbo.Companies");
            DropIndex("dbo.TestScores", new[] { "TestId" });
            DropIndex("dbo.CompanyManagements", new[] { "UserId" });
            DropIndex("dbo.CompanyManagements", new[] { "CompanyId" });
            DropColumn("dbo.Users", "DateOfBirth");
            DropTable("dbo.TestScores");
            DropTable("dbo.Tests");
            DropTable("dbo.CompanyManagements");
            DropTable("dbo.Companies");
        }
    }
}
