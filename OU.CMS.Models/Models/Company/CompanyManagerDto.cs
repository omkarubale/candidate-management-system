using OU.CMS.Models.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.Company
{
    public class CompanyManagerDto
    {
        public UserSimpleDto User { get; set; }

        public bool IsAdmin { get; set; }

        public bool HasAcceptedInvite { get; set; }

        public string InviteeEmail { get; set; }
    }
}
