// <auto-generated />
namespace OU.CMS.Domain.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class RemovedLengthConstraintOnPasswordHashAndSalt : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(RemovedLengthConstraintOnPasswordHashAndSalt));
        
        string IMigrationMetadata.Id
        {
            get { return "202010231455080_RemovedLengthConstraintOnPasswordHashAndSalt"; }
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