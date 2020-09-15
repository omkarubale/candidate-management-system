using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Entities;

namespace OU.CMS.Domain.Contexts
{
    public class CMSContext : DbContext
    {
        public CMSContext() : base("name=CMSConnection")
        {
                
        }

        public DbSet<User> Users { get; set; }
    }
}
