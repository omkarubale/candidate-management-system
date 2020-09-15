using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Entities;

namespace OU.CMS.Domain.EntityConfigurations
{
    public class CandidateConfiguration : EntityTypeConfiguration<Candidate>
    {
        public CandidateConfiguration()
        {
            HasRequired(e => e.User)
                .WithMany(e => e.Candidates)
                .HasForeignKey(e => e.UserId)
                .WillCascadeOnDelete(false);

            HasRequired(e => e.Company)
                .WithMany(e => e.Candidates)
                .HasForeignKey(e => e.CompanyId)
                .WillCascadeOnDelete(false);

            HasRequired(e => e.JobOpening)
                .WithMany(e => e.Candidates)
                .HasForeignKey(e => e.JobOpeningId)
                .WillCascadeOnDelete(false);
        }
    }
}
