using OU.CMS.Models.Authentication;
using OU.CMS.Web.API.Extensions;
using OU.CMS.Web.API.Filters;
using System.Web.Http;

namespace OU.CMS.Web.API.Controllers
{
    [JwtAuthentication]
    public class BaseSecureController : ApiController
    {
        public UserInfo UserInfo 
        { 
            get
            {
                var userInfo = new UserInfo()
                {
                    UserId = User.GetUserId(),
                    Email = User.GetEmail(),
                    FirstName = User.GetFirstName(),
                    LastName = User.GetLastName(),
                    FullName = User.GetFullName(),
                    ShortName = User.GetShortName(),
                    IsCandidateLogin = User.GetIsCandidateLogin(),
                    CompanyId = User.GetCompanyId(),
                };

                return userInfo;
            } 
        }
    }
}
