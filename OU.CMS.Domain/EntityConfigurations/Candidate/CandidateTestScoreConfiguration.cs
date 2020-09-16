using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Entities;

namespace OU.CMS.Domain.EntityConfigurations
{
    public class CandidateTestScoreConfiguration : EntityTypeConfiguration<CandidateTestScore>
    {
        public CandidateTestScoreConfiguration()
        {
            HasRequired(e => e.CandidateTest)
                .WithMany(e => e.CandidateTestScores)
                .HasForeignKey(e => e.CandidateTestId)
                .WillCascadeOnDelete(false);

            HasRequired(e => e.TestScore)
                .WithMany(e => e.CandidateTestScores)
                .HasForeignKey(e => e.TestScoreId)
                .WillCascadeOnDelete(false);
        }
    }
}
