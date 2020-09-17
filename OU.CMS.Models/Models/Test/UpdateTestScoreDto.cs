using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.Test
{
    public class UpdateTestScoreDto
    {
        public Guid Id { get; set; }

        public Guid TestId { get; set; }

        public string Title { get; set; }

        public bool IsMandatory { get; set; }
    }
}
