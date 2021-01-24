using OU.CMS.Core.BusinessLogic.CompanyManagers.Companies.Commands;
using OU.CMS.Core.BusinessLogic.CompanyManagers.Companies.Queries;
using OU.CMS.Core.BusinessLogic.CompanyManagers.CompanyManagements.Commands;
using OU.CMS.Core.BusinessLogic.CompanyManagers.CompanyManagements.Queries;
using OU.CMS.Models.Models.Company;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace OU.CMS.Web.API.Controllers.CompanyManagers
{
    public partial class ManagerCompanyController : BaseSecureController
    {
        #region Company
        [HttpGet]
        public async Task<GetCompanyDto> GetCompany(Guid companyId)
        {
            return await new GetCompanyQuery(companyId).GetCompany(UserInfo);
        }

        [HttpPost]
        public async Task<GetCompanyDto> CreateCompany(CreateCompanyDto dto)
        {
            return await new CreateCompanyCommand(dto).CreateCompany(UserInfo);
        }

        [HttpPost]
        public async Task<GetCompanyDto> EditCompany(EditCompanyDto dto)
        {
            return await new EditCompanyCommand(dto).EditCompany(UserInfo);
        }

        [HttpDelete]
        public async Task DeleteCompany(Guid companyId)
        {
            await new DeleteCompanyCommand(companyId).DeleteCompany(UserInfo);
        }
        #endregion

        #region CompanyManagement
        [HttpGet]
        public async Task<GetCompanyManagementDto> GetCompanyManagement(Guid companyId)
        {
            return await new GetCompanyManagementQuery(companyId).GetCompanyManagement(UserInfo);
        }

        [HttpPost]
        public async Task DeleteCompanyManagement(DeleteCompanyManagementDto dto)
        {
            await new DeleteCompanyManagementCommand(dto).DeleteCompanyManagement(UserInfo);
        }

        [HttpPut]
        public async Task CreateCompanyManagementInvite(CreateCompanyManagementInviteDto dto)
        {
            await new CreateCompanyManagementInviteCommand(dto).CreateCompanyManagementInvite(UserInfo);
        }

        [HttpPost]
        public async Task AcceptCompanyManagementInvite(AcceptCompanyManagementInviteDto dto)
        {
            await new AcceptCompanyManagementInviteCommand(dto).AcceptCompanyManagementInvite(UserInfo);
        }

        [HttpPost]
        public async Task RevokeCompanyManagementInvite(RevokeCompanyManagementInviteDto dto)
        {
            await new RevokeCompanyManagementInviteCommand(dto).RevokeCompanyManagementInvite(UserInfo);
        }
        #endregion
    }
}
