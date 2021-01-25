using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.User;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.Common.User.Queries
{
    public class GetIdentityUserQuery : BaseQuery<GetIdentityUserQuery>
    {
        public async Task<UserDto> GetIdentityUser(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                var user = await (from usr in db.Users
                                  where usr.Id == userInfo.UserId
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
    }
}
