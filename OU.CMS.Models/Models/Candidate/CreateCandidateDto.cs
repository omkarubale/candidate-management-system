using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.Candidate
{
    public class CreateCandidateDto
    {
        public Guid UserId { get; set; }

        public Guid CompanyId { get; set; }

        public Guid JobOpeningId { get; set; }
    }
}
