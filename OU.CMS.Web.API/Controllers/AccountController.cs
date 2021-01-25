using OU.CMS.Core.BusinessLogic.Common.Account.Commands;
using OU.CMS.Core.BusinessLogic.Common.Account.Queries;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Account;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace OU.CMS.Web.API.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<UserInfo> Login(SignInDto dto)
        {
            var userInfo = await new LoginCommand(dto).Login();

            userInfo.Token = JwtManager.GenerateToken(userInfo, out var expiresOn, 180, userInfo.CompanyId);
            userInfo.ExpiresOn = expiresOn;

            return userInfo;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Register(SignUpDto dto)
        {
            await new RegisterCommand(dto).Register();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<RegisterByInvitePageDto> GetRegisterByInvitePage(string email, Guid companyId)
        {
            return await new GetRegisterByInvitePageQuery(email, companyId).GetRegisterByInvitePage();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task RegisterByManagerInvite(SignUpByManagerInviteDto dto)
        {
            await new RegisterByManagerInviteCommand(dto).RegisterByManagerInvite();
        }
    }
}