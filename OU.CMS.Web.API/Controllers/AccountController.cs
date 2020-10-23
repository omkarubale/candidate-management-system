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
        private static string Secret = "5C0E753CA285F031D86A5496E857D70FDE170EE42F61734391CEF0DA8A677F44E68E859E5BE8ACE93D605128CC8B4BD8B03F9D15A7B72953A7FF45142FB6EA8037A5834BDB8C9D1E69899F1DCCE9F15134FC7021EEEF4A01A1E229B17CEA5FF1BF43AEE1702F4283928551AA24F83C81A3247DB2C8F0";

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

                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.FullName),
                    //new Claim(ClaimTypes.) // Create partial class for adding custom claims
                };

                var key = Convert.FromBase64String(Secret);
                var securitykey = new SymmetricSecurityKey(key);

                var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                var token = tokenHandler.CreateToken(tokenDescriptor);

                // Instead of returning this, put all of this in Token using Claims
                return new UserInfo() { 
                    Token = tokenHandler.WriteToken(token),
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ShortName = user.ShortName,
                    FullName = user.FullName,
                    UserType = user.UserType,
                    IsCandidateLogin = user.UserType == UserType.Candidate,
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

        [HttpGet]
        [AllowAnonymous]
        public async Task SetPassword(Guid userId, string password)
        {
            using (var db = new CMSContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.Id == userId);

                // User doesn't exist
                if (user == null)
                    throw new Exception("User with this email doesn't exist!");

                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHash;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch(Exception e)
                {
                    throw e;
                }
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