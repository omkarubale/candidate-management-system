using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Lookups;

namespace OU.CMS.Domain.Entities
{
    public class User : BaseEntity<Guid>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string Email { get; set; }

        public UserType UserType { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public IList<Candidate> Candidates { get; set; }
        public IList<CompanyManagement> CompanyManagements { get; set; }
        public IList<Candidate> InterviewCandidates { get; set; }
    }
}
