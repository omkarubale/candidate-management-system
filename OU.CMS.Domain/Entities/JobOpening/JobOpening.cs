using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Domain.Entities
{
    public class JobOpening : BaseEntity<Guid>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public decimal Salary { get; set; }

        public DateTime Deadline { get; set; }

        public IList<Candidate> Candidates { get; set; }

        //Created Log
        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }


    }
}
