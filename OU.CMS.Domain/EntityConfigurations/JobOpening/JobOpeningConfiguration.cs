using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Entities;

namespace OU.CMS.Domain.EntityConfigurations
{ 
    public class JobOpeningConfiguration : EntityTypeConfiguration<JobOpening>
    {
        public JobOpeningConfiguration()
        {
            HasRequired(e => e.Company)
                .WithMany(e => e.JobOpenings)
                .HasForeignKey(e => e.CompanyId)
                .WillCascadeOnDelete(false);
        }
    }
}
