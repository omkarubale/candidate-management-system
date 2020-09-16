using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Domain.Entities
{
    public class CandidateTest : BaseEntity<Guid>
    {
        public Guid CandidateId { get; set; }

        [ForeignKey(nameof(CandidateId))]
        public Candidate Candidate { get; set; }

        public Guid TestId { get; set; }

        [ForeignKey(nameof(TestId))]
        public Test Test { get; set; }

        public IList<CandidateTestScore> CandidateTestScores { get; set; }
    }
}
