namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyManagementInviteTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CompanyManagementInvites",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CompanyId = c.Guid(nullable: false),
                        Email = c.String(nullable: false, maxLength: 255),
                        IsInviteForAdmin = c.Boolean(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                        CreatedBy = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId)
                .Index(t => t.CompanyId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CompanyManagementInvites", "CompanyId", "dbo.Companies");
            DropIndex("dbo.CompanyManagementInvites", new[] { "CompanyId" });
            DropTable("dbo.CompanyManagementInvites");
        }
    }
}
