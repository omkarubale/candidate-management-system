using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Domain.Entities
{
    public class Test : BaseEntity<Guid>
    {
        public string Title { get; set; }

        public IList<TestScore> TestScores { get; set; }

        public IList<CandidateTest> CandidateTests { get; set; }

        //Created Log
        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }
    }
}
