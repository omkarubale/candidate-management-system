// <auto-generated />
namespace OU.CMS.Domain.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class AddedDefaultCompanyIdForUserAndRemovedPasswordFields : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddedDefaultCompanyIdForUserAndRemovedPasswordFields));
        
        string IMigrationMetadata.Id
        {
            get { return "202010231435158_AddedDefaultCompanyIdForUserAndRemovedPasswordFields"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}