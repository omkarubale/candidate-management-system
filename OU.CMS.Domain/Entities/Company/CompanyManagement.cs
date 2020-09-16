using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Domain.Entities
{
    public class CompanyManagement : BaseEntity<Guid>
    {
        public Guid CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public bool IsAdmin { get; set; }

        //Created Log
        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }
    }
}
