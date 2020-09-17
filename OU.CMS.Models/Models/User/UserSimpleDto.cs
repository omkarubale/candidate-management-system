using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.User
{
    public class UserSimpleDto
    {
        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public string Email { get; set; }
    }
}
