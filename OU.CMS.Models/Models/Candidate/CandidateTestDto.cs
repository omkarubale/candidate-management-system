using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Models.Models.Common;

namespace OU.CMS.Models.Models.Candidate
{
    public class CandidateTestDto
    {
        public Guid CandidateId { get; set; }

        public string Title { get; set; }

        public List<CandidateTestScoreDto> CandidateTestScores { get; set; }
    }
}
