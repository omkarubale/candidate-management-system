using Microsoft.Ajax.Utilities;
using OU.CMS.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using Claim = System.Security.Claims.Claim;

namespace OU.CMS.Web.API.Extensions
{
    public static partial class Extensions
    {
        private static string GetClaimValue(IPrincipal user, Func<Claim, bool> selector)
        {
            var userClaims = (ClaimsPrincipal)user;

            return userClaims.Claims.SingleOrDefault(selector)?.Value;
        }

        public static Guid GetUserId(this IPrincipal user)
        {
            return new Guid(GetClaimValue(user, i => i.Type == CustomClaimTypes.UserId));
        }

        public static string GetEmail(this IPrincipal user)
        {
            return GetClaimValue(user, i => i.Type == CustomClaimTypes.Email);
        }

        public static string GetFirstName(this IPrincipal user)
        {
            return GetClaimValue(user, i => i.Type == CustomClaimTypes.FirstName);
        }

        public static string GetLastName(this IPrincipal user)
        {
            return GetClaimValue(user, i => i.Type == CustomClaimTypes.LastName);
        }

        public static string GetFullName(this IPrincipal user)
        {
            return GetClaimValue(user, i => i.Type == CustomClaimTypes.FullName);
        }

        public static string GetShortName(this IPrincipal user)
        {
            return GetClaimValue(user, i => i.Type == CustomClaimTypes.ShortName);
        }

        public static bool GetIsCandidateLogin(this IPrincipal user)
        {
            bool.TryParse(GetClaimValue(user, i => i.Type == CustomClaimTypes.IsCandidateLogin), out var result);
            return result;
        }

        public static Guid? GetCompanyId(this IPrincipal user)
        {
            Guid.TryParse(GetClaimValue(user, i => i.Type == CustomClaimTypes.CompanyId), out var result);
            return result;
        }
    }
}