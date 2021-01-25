using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.JobOpening
{
    public class CreateJobOpeningDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Salary { get; set; }

        public DateTime Deadline { get; set; }
    }
}
