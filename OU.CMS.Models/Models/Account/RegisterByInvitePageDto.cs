using System;

namespace OU.CMS.Models.Models.Account
{
    public class RegisterByInvitePageDto
    {
        public string Email { get; set; }

        public Guid CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string InvitedByUserName { get; set; }       
    }
}
