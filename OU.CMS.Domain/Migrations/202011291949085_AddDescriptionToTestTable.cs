namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionToTestTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tests", "Description", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tests", "Description");
        }
    }
}
