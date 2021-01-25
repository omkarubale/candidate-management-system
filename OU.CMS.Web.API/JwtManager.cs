using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using OU.CMS.Common;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Authentication;

namespace OU.CMS.Web.API
{
    public class JwtManager
    {
        /// <summary>
        /// Use the below code to generate symmetric Secret Key
        ///     var hmac = new HMACSHA256();
        ///     var key = Convert.ToBase64String(hmac.Key);
        /// </summary>
        public static string GenerateToken(UserInfo userInfo, out DateTime expiresOn, int expireMinutes = 20, Guid? companyId = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            expiresOn = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireMinutes));

            var claims = new List<Claim>() {
                new Claim(CustomClaimTypes.UserId, userInfo.UserId.ToString()),
                new Claim(CustomClaimTypes.Email, userInfo.Email),
                new Claim(CustomClaimTypes.FirstName, userInfo.FirstName),
                new Claim(CustomClaimTypes.LastName, userInfo.LastName),
                new Claim(CustomClaimTypes.FullName, userInfo.FullName),
                new Claim(CustomClaimTypes.ShortName, userInfo.ShortName),
                new Claim(CustomClaimTypes.UserType, userInfo.UserType.ToString()),
                new Claim(CustomClaimTypes.IsCandidateLogin, companyId == null ? true.ToString() : false.ToString()),
                new Claim(CustomClaimTypes.CompanyId, companyId.ToString()),
                new Claim(CustomClaimTypes.ExpiresOn, expiresOn.ToString()),
            };

            var symmetricKey = Convert.FromBase64String(Constants.Secret);
            var securitykey = new SymmetricSecurityKey(symmetricKey);

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresOn,
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
            catch
            {
                return null;
            }
        }
    }
}