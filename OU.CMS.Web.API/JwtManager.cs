using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OU.CMS.Common;
using OU.CMS.Domain.Entities;

namespace OU.CMS.Web.API
{
    public class JwtManager
    {
        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
        public static string GenerateToken(User user, int expireMinutes = 20, Guid? companyId = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = new List<Claim>() {
                new Claim(CustomClaimTypes.UserId, user.Id.ToString()),
                new Claim(CustomClaimTypes.Email, user.Email),
                new Claim(CustomClaimTypes.FirstName, user.FirstName),
                new Claim(CustomClaimTypes.LastName, user.LastName),
                new Claim(CustomClaimTypes.FullName, user.FullName),
                new Claim(CustomClaimTypes.ShortName, user.ShortName),
                new Claim(CustomClaimTypes.UserType, user.UserType.ToString()),
                new Claim(CustomClaimTypes.IsCandidateLogin, companyId == null ? true.ToString() : false.ToString()),
                new Claim(CustomClaimTypes.CompanyId, companyId.ToString()),
            };

            var symmetricKey = Convert.FromBase64String(Constants.Secret);
            var securitykey = new SymmetricSecurityKey(symmetricKey);

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireMinutes)),
                SigningCredentials = credentials
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(securityToken);

            return token;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Constants.Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}