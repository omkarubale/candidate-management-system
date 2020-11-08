using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;

namespace OU.CMS.Models.Models.JobOpening
{
    public class GetCandidateJobOpeningDto
    {
        public Guid UserId { get; set; }

        public Guid JobOpeningId { get; set; }

        public Guid CandidateId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Salary { get; set; }

        public DateTime Deadline { get; set; }

        public DateTime AppliedOn { get; set; }

        public CompanySimpleDto Company { get; set; }

        public CreatedOnDto CreatedDetails { get; set; }
    }
}
