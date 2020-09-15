using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Domain.Entities
{
    public class Test : BaseEntity<Guid>
    {
        public string Title { get; set; }

        public List<TestScore> TestScores { get; set; }
    }
}
