using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Domain.Entities
{
    public class CompanyManagementInvite : BaseEntity<Guid>
    {
        public Guid CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public string Email { get; set; }

        public bool IsInviteForAdmin { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }
    }
}
