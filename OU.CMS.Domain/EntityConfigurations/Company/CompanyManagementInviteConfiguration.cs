using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Entities;

namespace OU.CMS.Domain.EntityConfigurations
{
    public class CompanyManagementInviteConfiguration : EntityTypeConfiguration<CompanyManagementInvite>
    {
        public CompanyManagementInviteConfiguration()
        {
            Property(v => v.Email)
                .IsRequired()
                .HasMaxLength(255);

            HasRequired(e => e.Company)
                .WithMany(e => e.CompanyManagementInvites)
                .HasForeignKey(e => e.CompanyId)
                .WillCascadeOnDelete(false);
        }
    }
}
