using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.Test
{
    public class TestScoreDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool IsMandatory { get; set; }

        public decimal MinimumScore { get; set; }

        public decimal MaximumScore { get; set; }

        public decimal? CutoffScore { get; set; }
    }
}
