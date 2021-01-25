using OU.CMS.Core.BusinessLogic.Common.User.Commands;
using OU.CMS.Core.BusinessLogic.Common.User.Queries;
using OU.CMS.Models.Models.User;
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
            return await new GetIdentityUserQuery().GetIdentityUser(UserInfo);
        }

        [HttpPost]
        public async Task<UserDto> SaveIdentityUser(UserDto dto)
        {
            return await new SaveIdentityUserCommand(dto).SaveIdentityUser(UserInfo);
        }
        #endregion
    }
}
