using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Domain.Entities
{
    public class Company : BaseEntity<Guid>
    {
        public string Name { get; set; }

        //Created Log
        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public IList<CompanyManagement> CompanyManagements { get; set; }
        public IList<CompanyManagementInvite> CompanyManagementInvites { get; set; }
        public IList<Candidate> Candidates { get; set; }
        public IList<JobOpening> JobOpenings { get; set; }
        public IList<User> UsersWithDefaultCompany { get; set; }
        public IList<Test> Tests { get; set; }
    }
}
