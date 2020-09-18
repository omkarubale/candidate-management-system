using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Domain.Entities
{
    public class TestScore : BaseEntity<Guid>
    {
        public string Title { get; set; }

        public bool IsMandatory { get; set; }

        public decimal MinimumScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal? CutoffScore { get; set; }

        public Guid TestId { get; set; }

        [ForeignKey(nameof(TestId))]
        public Test Test { get; set; }

        public IList<CandidateTestScore> CandidateTestScores { get; set; }
    }
}
