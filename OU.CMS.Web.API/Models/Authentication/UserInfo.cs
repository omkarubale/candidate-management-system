using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using OU.CMS.Domain.Lookups;

namespace OU.CMS.Web.API.Models.Authentication
{
    public class UserInfo
    {
        public SecurityToken JwtToken { get; set; }

        public Guid UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public UserType UserType { get; set; }

        //Only present when user is a companyManager
        public Guid? CompanyId { get; set; }

        //TODO: Add more fields to UserInfo
    }
}