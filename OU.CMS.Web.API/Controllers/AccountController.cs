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

namespace OU.CMS.Web.API.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IConfiguration _config;

        [HttpPost]
        [AllowAnonymous]
        public async Task<UserInfo> Login(SignInDto loginDto)
        {
            loginDto.Email = loginDto.Email.Trim().ToLower();

            using (var db = new CMSContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email && u.UserType == loginDto.UserType);

                // User doesn't exist
                if (user == null)
                    throw new Exception("User does not exist or Password is not correct!");

                // Password Incorrect
                if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
                    throw new Exception("User does not exist or Password is not correct!");

                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FullName),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new UserInfo() { 
                    JwtToken = token,
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ShortName = user.ShortName,
                    FullName = user.FullName,
                    UserType = user.UserType,
                    CompanyId = user.UserType == UserType.Management ? user.DefaultCompanyId : null
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
                if (registerDto.IsPasswordMatch)
                    throw new Exception("Password confirmation does not match!");

                

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(registerDto.Password, out passwordHash, out passwordSalt);

                user = new Domain.Entities.User()
                {
                    Id = Guid.NewGuid(),
                    Email = registerDto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    UserType = registerDto.UserType,

                    FullName = registerDto.FullName,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    ShortName = registerDto.ShortName,
                    DateOfBirth = registerDto.DateOfBirth,

                    // TODO: Add Default company when getting invite for company Manager
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
        }

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