using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Domain.Entities
{
    public class CandidateTestScore : BaseEntity<Guid>
    {
        public Guid CandidateTestId { get; set; }

        public CandidateTest CandidateTest { get; set; }

        public Guid TestScoreId { get; set; }

        public TestScore TestScore { get; set; }

        public decimal Value { get; set; }

        public string Comment { get; set; }
    }
}
