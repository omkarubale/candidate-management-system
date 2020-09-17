using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.JobOpening;
using OU.CMS.Models.Models.User;

namespace OU.CMS.Models.Models.Candidate
{
    public class GetCandidateDto
    {
        public UserSimpleDto User { get; set; }

        public CompanySimpleDto Company { get; set; }

        public JobOpeningSimpleDto JobOpening { get; set; }

        public CreatedOnDto CreatedDetails { get; set; }
    }
}
