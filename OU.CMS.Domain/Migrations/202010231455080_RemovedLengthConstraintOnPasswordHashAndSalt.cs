namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedLengthConstraintOnPasswordHashAndSalt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "PasswordHash", c => c.Binary(nullable: false));
            AlterColumn("dbo.Users", "PasswordSalt", c => c.Binary(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "PasswordSalt", c => c.Binary(nullable: false, maxLength: 100));
            AlterColumn("dbo.Users", "PasswordHash", c => c.Binary(nullable: false, maxLength: 100));
        }
    }
}
