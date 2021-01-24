using OU.CMS.Core.BusinessLogic.Candidates.Companies.Queries;
using OU.CMS.Models.Models.Company;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace OU.CMS.Web.API.Controllers.Candidates
{
    public partial class CandidateCompanyController : BaseSecureController
    {
        #region Company
        [HttpGet]
        public async Task<List<GetCompanyDto>> GetAllCompanies()
        {
            return await new GetAllCompaniesQuery().GetAllCompanies(UserInfo);
        }
        #endregion
    }
}