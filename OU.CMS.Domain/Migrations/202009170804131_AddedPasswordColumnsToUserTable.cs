namespace OU.CMS.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPasswordColumnsToUserTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PasswordTemp", c => c.String());
            AddColumn("dbo.Users", "PasswordHash", c => c.String());
            AddColumn("dbo.Users", "PasswordSalt", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PasswordSalt");
            DropColumn("dbo.Users", "PasswordHash");
            DropColumn("dbo.Users", "PasswordTemp");
        }
    }
}
