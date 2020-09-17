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

        #region Candidate
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateTest> CandidateTests { get; set; }
        public DbSet<CandidateTestScore> CandidateTestScores { get; set; }
        #endregion

        #region Company
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyManagement> CompanyManagements { get; set; }
        public DbSet<CompanyManagementInvite> CompanyManagementInvites { get; set; }
        #endregion

        #region JobOpening
        public DbSet<JobOpening> JobOpenings { get; set; }
        #endregion

        #region Test
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestScore> TestScores { get; set; }
        #endregion

        #region User
        public DbSet<User> Users { get; set; }
        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CandidateConfiguration());
            modelBuilder.Configurations.Add(new CandidateTestConfiguration());
            modelBuilder.Configurations.Add(new CandidateTestScoreConfiguration());

            modelBuilder.Configurations.Add(new CompanyConfiguration());
            modelBuilder.Configurations.Add(new CompanyManagementConfiguration());
            modelBuilder.Configurations.Add(new CompanyManagementInviteConfiguration());

            modelBuilder.Configurations.Add(new JobOpeningConfiguration());

            modelBuilder.Configurations.Add(new TestConfiguration());
            modelBuilder.Configurations.Add(new TestScoreConfiguration());

            modelBuilder.Configurations.Add(new UserConfiguration());
        }
    }
}
