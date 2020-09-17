using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Models.Models.Common;

namespace OU.CMS.Models.Models.Test
{
    public class GetTestDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public List<TestScoreDto> TestScores { get; set; }

        public CreatedOnDto CreatedDetails { get; set; }
    }
}
