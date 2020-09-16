using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Entities;

namespace OU.CMS.Domain.EntityConfigurations
{
    class CandidateTestConfiguration : EntityTypeConfiguration<CandidateTest>
    {
        public CandidateTestConfiguration()
        {
            HasRequired(e => e.Candidate)
                .WithMany(e => e.CandidateTests)
                .HasForeignKey(e => e.CandidateId)
                .WillCascadeOnDelete(false);

            HasRequired(e => e.Test)
                .WithMany(e => e.CandidateTests)
                .HasForeignKey(e => e.TestId)
                .WillCascadeOnDelete(false);
        }
    }
}
