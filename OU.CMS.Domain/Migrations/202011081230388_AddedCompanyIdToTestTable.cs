namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCompanyIdToTestTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tests", "CompanyId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Tests", "CompanyId");
            AddForeignKey("dbo.Tests", "CompanyId", "dbo.Companies", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tests", "CompanyId", "dbo.Companies");
            DropIndex("dbo.Tests", new[] { "CompanyId" });
            DropColumn("dbo.Tests", "CompanyId");
        }
    }
}
