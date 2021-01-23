using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace OU.CMS.Web.API.Controllers
{
    public class UserController : BaseSecureController
    {
        #region User
        [HttpGet]
        public async Task<UserDto> GetIdentityUser()
        {
            using (var db = new CMSContext())
            {
                var user = await (from usr in db.Users
                                  where usr.Id == UserInfo.UserId
                                  select new UserDto
                                  {
                                      FirstName = usr.FirstName,
                                      LastName = usr.LastName,
                                      FullName = usr.FullName,
                                      ShortName = usr.ShortName,
                                      Email = usr.Email,
                                  }).SingleOrDefaultAsync();

                if (user == null)
                    throw new Exception("User does not exist!");

                return user;
            }
        }

        [HttpPost]
        public async Task<UserDto> SaveIdentityUser(UserDto dto)
        {
            using (var db = new CMSContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(c => c.Id == UserInfo.UserId);
                if (user == null)
                    throw new Exception("Company with Id not found!");

                user.FirstName = dto.FirstName.Trim();
                user.LastName = dto.LastName.Trim();
                user.Email = dto.Email.Trim();

                await db.SaveChangesAsync();

                return dto;
            }
        }
        #endregion
    }
}
