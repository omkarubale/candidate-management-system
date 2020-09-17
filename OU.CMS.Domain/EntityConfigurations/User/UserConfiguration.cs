using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Entities;

namespace OU.CMS.Domain.EntityConfigurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            Property(v => v.FirstName)
                .IsRequired()
                .HasMaxLength(150);

            Property(v => v.LastName)
                .IsRequired()
                .HasMaxLength(150);

            Property(v => v.FullName)
                .IsRequired()
                .HasMaxLength(300);

            Property(v => v.ShortName)
                .IsRequired()
                .HasMaxLength(3);

            Property(v => v.Email)
                .IsRequired()
                .HasMaxLength(255);

            Property(v => v.PasswordTemp)
                .IsRequired()
                .HasMaxLength(25);

            Property(v => v.PasswordSalt)
                .IsRequired()
                .HasMaxLength(100);

            Property(v => v.PasswordHash)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
