using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Models.Models.Common;

namespace OU.CMS.Models.Models.Company
{
    public class GetCompanyDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public CreatedOnDto CreatedDetails { get; set; }
    }
}
