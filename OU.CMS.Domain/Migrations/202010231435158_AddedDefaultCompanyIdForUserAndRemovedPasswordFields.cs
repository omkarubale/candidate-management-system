namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDefaultCompanyIdForUserAndRemovedPasswordFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "DefaultCompanyId", c => c.Guid());
            CreateIndex("dbo.Users", "DefaultCompanyId");
            AddForeignKey("dbo.Users", "DefaultCompanyId", "dbo.Companies", "Id");
            DropColumn("dbo.Users", "PasswordTemp");
            DropColumn("dbo.Users", "PasswordHash");
            DropColumn("dbo.Users", "PasswordSalt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "PasswordSalt", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Users", "PasswordHash", c => c.String(nullable: false, maxLength: 100));
            AddColumn("dbo.Users", "PasswordTemp", c => c.String(nullable: false, maxLength: 25));
            DropForeignKey("dbo.Users", "DefaultCompanyId", "dbo.Companies");
            DropIndex("dbo.Users", new[] { "DefaultCompanyId" });
            DropColumn("dbo.Users", "DefaultCompanyId");
        }
    }
}
