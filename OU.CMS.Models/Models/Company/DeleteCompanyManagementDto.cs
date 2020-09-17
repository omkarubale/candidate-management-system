using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.Company
{
    public class DeleteCompanyManagementDto
    {
        public Guid CompanyId { get; set; }

        public Guid UserId { get; set; }
    }
}
