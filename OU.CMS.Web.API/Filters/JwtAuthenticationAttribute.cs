using OU.CMS.Common;
using OU.CMS.Domain.Lookups;
using OU.CMS.Web.API.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Web;

namespace OU.CMS.Web.API.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
                return;

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            var token = authorization.Parameter;
            var principal = await AuthenticateJwtToken(token);

            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
            else
            {
                context.Principal = principal;
            }
        }

        private static bool ValidateToken(string token, out UserDetails userDetails)
        {
            userDetails = new UserDetails();

            var simplePrinciple = JwtManager.GetPrincipal(token);
            var identity = simplePrinciple?.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var userIdClaim = identity.FindFirst(CustomClaimTypes.UserId);
            var userId = userIdClaim?.Value;

            if (string.IsNullOrEmpty(userId))
                return false;

            userDetails.UserId = new Guid(userId);
            userDetails.Email = identity.FindFirst(CustomClaimTypes.Email)?.Value;
            userDetails.FirstName = identity.FindFirst(CustomClaimTypes.FirstName)?.Value;
            userDetails.LastName = identity.FindFirst(CustomClaimTypes.LastName)?.Value;
            userDetails.FullName = identity.FindFirst(CustomClaimTypes.FullName)?.Value;
            userDetails.ShortName = identity.FindFirst(CustomClaimTypes.ShortName)?.Value;
            userDetails.IsCandidateLogin = identity.FindFirst(CustomClaimTypes.IsCandidateLogin)?.Value == "True";
            userDetails.ExpiresOn = DateTime.Parse(identity.FindFirst(CustomClaimTypes.ExpiresOn)?.Value);

            var companyId = identity.FindFirst(CustomClaimTypes.CompanyId)?.Value;

            if (!string.IsNullOrEmpty(companyId) && !userDetails.IsCandidateLogin)
                userDetails.CompanyId = new Guid(companyId);

            // More validate to check whether username exists in system

            return true;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            if (ValidateToken(token, out var userDetails))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>() {
                    new Claim(CustomClaimTypes.UserId, userDetails.UserId.ToString()),
                    new Claim(CustomClaimTypes.Email, userDetails.Email),
                    new Claim(CustomClaimTypes.FirstName, userDetails.FirstName),
                    new Claim(CustomClaimTypes.LastName, userDetails.LastName),
                    new Claim(CustomClaimTypes.FullName, userDetails.FullName),
                    new Claim(CustomClaimTypes.ShortName, userDetails.ShortName),
                    new Claim(CustomClaimTypes.IsCandidateLogin, userDetails.IsCandidateLogin.ToString()),
                    new Claim(CustomClaimTypes.CompanyId, userDetails.CompanyId.ToString()),
                    new Claim(CustomClaimTypes.ExpiresOn, userDetails.ExpiresOn.ToString()),
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }
    }
}