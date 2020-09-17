namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDeadlineColumnToJobOpeningTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JobOpenings", "Deadline", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JobOpenings", "Deadline");
        }
    }
}
