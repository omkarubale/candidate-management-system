using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Entities;

namespace OU.CMS.Domain.EntityConfigurations
{
    public class TestScoreConfiguration : EntityTypeConfiguration<TestScore>
    {
        public TestScoreConfiguration()
        {
            HasRequired(e => e.Test)
                .WithMany(e => e.TestScores)
                .HasForeignKey(e => e.TestId)
                .WillCascadeOnDelete(false);
        }
    }
}
