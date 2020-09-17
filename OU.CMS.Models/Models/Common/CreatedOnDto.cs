using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.Common
{
    public class CreatedOnDto
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
