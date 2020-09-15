using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Domain.Entities
{
    public class Company : BaseEntity<Guid>
    {
        public string Name { get; set; }
    }
}
