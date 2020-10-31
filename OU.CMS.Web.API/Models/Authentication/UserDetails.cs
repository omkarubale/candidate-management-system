using OU.CMS.Domain.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OU.CMS.Web.API.Models.Authentication
{
    public class UserDetails
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        //Only present when user is a companyManager
        public bool IsCandidateLogin { get; set; }

        public Guid? CompanyId { get; set; }
    }
}