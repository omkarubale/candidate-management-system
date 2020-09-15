using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Entities;
using OU.CMS.Domain.EntityConfigurations;

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

        #region Candidate
        public DbSet<Candidate> Candidates { get; set; }
        #endregion

        #region JobOpening
        public DbSet<JobOpening> JobOpenings { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());

            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new CompanyManagementConfiguration());

            modelBuilder.Configurations.Add(new TestConfiguration());

            modelBuilder.Configurations.Add(new CandidateConfiguration());
        }
    }
}
