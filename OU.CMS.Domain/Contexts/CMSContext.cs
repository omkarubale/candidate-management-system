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

        #region User
        public DbSet<User> Users { get; set; }
        #endregion

        #region Company
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyManagement> CompanyManagements { get; set; }
        #endregion

        #region Test
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestScore> TestScores { get; set; }
        #endregion

    }
}
