namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedStringLimitsForAllTables : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CandidateTestScores", "Comment", c => c.String(nullable: false, maxLength: 250));
            AlterColumn("dbo.TestScores", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Tests", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Companies", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "FirstName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Users", "LastName", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.Users", "FullName", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Users", "ShortName", c => c.String(nullable: false, maxLength: 3));
            AlterColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Users", "PasswordTemp", c => c.String(nullable: false, maxLength: 25));
            AlterColumn("dbo.Users", "PasswordHash", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "PasswordSalt", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.JobOpenings", "Title", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.JobOpenings", "Title", c => c.String());
            AlterColumn("dbo.Users", "PasswordSalt", c => c.String());
            AlterColumn("dbo.Users", "PasswordHash", c => c.String());
            AlterColumn("dbo.Users", "PasswordTemp", c => c.String());
            AlterColumn("dbo.Users", "Email", c => c.String());
            AlterColumn("dbo.Users", "ShortName", c => c.String());
            AlterColumn("dbo.Users", "FullName", c => c.String());
            AlterColumn("dbo.Users", "LastName", c => c.String());
            AlterColumn("dbo.Users", "FirstName", c => c.String());
            AlterColumn("dbo.Companies", "Name", c => c.String());
            AlterColumn("dbo.Tests", "Title", c => c.String());
            AlterColumn("dbo.TestScores", "Title", c => c.String());
            AlterColumn("dbo.CandidateTestScores", "Comment", c => c.String());
        }
    }
}
