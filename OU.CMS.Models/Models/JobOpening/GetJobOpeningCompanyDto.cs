using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.JobOpening
{
    public class GetJobOpeningCompanyDto : GetJobOpeningDto
    {
        public int CandidateCount { get; set; }
    }
}
