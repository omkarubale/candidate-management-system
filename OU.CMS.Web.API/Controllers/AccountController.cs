using System;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.Account;
using OU.CMS.Web.API.Models.Authentication;
using OU.CMS.Domain.Lookups;
using OU.CMS.Common;
using System.Collections.Generic;
using System.Linq;

namespace OU.CMS.Web.API.Controllers
{
    public class AccountController : ApiController
    {

        [HttpPost]
        [AllowAnonymous]
        public async Task<UserInfo> Login(SignInDto loginDto)
        {
            loginDto.Email = loginDto.Email.Trim().ToLower();

            using (var db = new CMSContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email && u.UserType == (UserType)loginDto.UserType);

                // User doesn't exist
                if (user == null)
                    throw new Exception("User does not exist or Password is not correct!");

                // Password Incorrect
                if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                    throw new Exception("User does not exist or Password is not correct!");

                var companyId = user.UserType == UserType.Management ? user.DefaultCompanyId : null;

                return new UserInfo()
                {
                    Token = JwtManager.GenerateToken(user, 30, companyId),
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ShortName = user.ShortName,
                    FullName = user.FullName,
                    UserType = user.UserType,
                    IsCandidateLogin = user.UserType == UserType.Candidate,
                    CompanyId = companyId
                    //TODO: Add more fields to UserInfo
                };
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Register(SignUpDto registerDto)
        {
            using (var db = new CMSContext())
            {
				var user = await db.Users.SingleOrDefaultAsync(u => u.Email == registerDto.Email);

                // User doesn't exist
                if (user != null)
                    throw new Exception("User with this email already exists!");

                // Password Incorrect
                if (!registerDto.IsPasswordMatch)
                    throw new Exception("Password confirmation does not match!");

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

                var x = registerDto.FirstName.ToCharArray().First().ToString() + registerDto.LastName.ToCharArray().First().ToString();

                user = new Domain.Entities.User()
                {
                    Id = Guid.NewGuid(),
                    Email = registerDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    UserType = registerDto.UserType,

                    FullName = registerDto.FirstName + " " + registerDto.LastName,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    ShortName = (registerDto.FirstName.ToCharArray().First().ToString() + registerDto.LastName.ToCharArray().First().ToString()).ToUpper(),
                    // TODO: Add Default company when getting invite for company Manager
                };

                db.Users.Add(user);

                if (!registerDto.IsCandidateLogin)
                {
                    var company = new Domain.Entities.Company()
                    {
                        Id = Guid.NewGuid(),
                        Name = registerDto.CompanyName
                    };

                    var companyManagement = new Domain.Entities.CompanyManagement()
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = company.Id,
                        UserId = user.Id,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = user.Id
                    };

                    db.Companies.Add(company);
                    db.CompanyManagements.Add(companyManagement);
                }
                
                await db.SaveChangesAsync();
            }
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task SetPassword(Guid userId, string password)
        //{
        //    using (var db = new CMSContext())
        //    {
        //        var user = await db.Users.SingleOrDefaultAsync(u => u.Id == userId);

        //        // User doesn't exist
        //        if (user == null)
        //            throw new Exception("User with this email doesn't exist!");

        //        byte[] passwordHash, passwordSalt;
        //        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        //        user.PasswordSalt = passwordSalt;
        //        user.PasswordHash = passwordHash;
        //        try
        //        {
        //            await db.SaveChangesAsync();
        //        }
        //        catch(Exception e)
        //        {
        //            throw e;
        //        }
        //    }
        //}

        #region System Password Methods
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }
        #endregion
    }
}