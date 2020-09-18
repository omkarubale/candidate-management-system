using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.Candidate
{
    public class CandidateTestScoreDto
    {
        public Guid CandidateTestScoreId { get; set; }

        public Guid TestScoreId { get; set; }

        public string Title { get; set; }

        public bool IsMandatory { get; set; }

        public decimal? Value { get; set; }

        public string Comment { get; set; }
    }
}
