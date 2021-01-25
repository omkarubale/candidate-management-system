using OU.CMS.Core.BusinessLogic.Common.Account.Commands;
using OU.CMS.Models.Models.Account;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OU.CMS.Web.API.Controllers
{
    public class AccountSecureController : BaseSecureController
    {
        [HttpPost]
        public async Task UpdatePassword(UpdatePasswordDto dto)
        {
            await new UpdatePasswordCommand(dto).UpdatePassword(UserInfo);
        }
    }
}