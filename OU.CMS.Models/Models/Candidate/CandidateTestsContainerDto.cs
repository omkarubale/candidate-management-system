using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.Candidate
{
    public class CandidateTestsContainerDto
    {
        public GetCandidateDto Candidate { get; set; }

        public List<CandidateTestDto> CandidateTests { get; set; }
    }
}
