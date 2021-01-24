using OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Commands;
using OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Queries;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.JobOpening;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace OU.CMS.Web.API.Controllers.CompanyManagers
{
    public class ManagerJobController : BaseSecureController
    {
        #region JobOpening
        [HttpGet]
        public async Task<List<GetJobOpeningCompanyDto>> GetJobOpenings()
        {
            return await new GetJobOpeningsQuery().GetJobOpenings(UserInfo);
        }

        [HttpGet]
        public async Task<GetJobOpeningDto> GetJobOpening(Guid jobOpeningId)
        {
            return await new GetJobOpeningQuery(jobOpeningId).GetJobOpening(UserInfo);
        }

        [HttpPost]
        public async Task<GetJobOpeningDto> CreateJobOpening(CreateJobOpeningDto dto)
        {
            return await new CreateJobOpeningCommand(dto).CreateJobOpening(UserInfo);
        }

        [HttpPost]
        public async Task<GetJobOpeningDto> UpdateJobOpening(UpdateJobOpeningDto dto)
        {
            return await new UpdateJobOpeningCommand(dto).UpdateJobOpening(UserInfo);
        }

        [HttpDelete]
        public async Task DeleteJobOpening(Guid jobOpeningId)
        {
            await new DeleteJobOpeningCommand(jobOpeningId).DeleteJobOpening(UserInfo);
        }
        #endregion

    }
}
