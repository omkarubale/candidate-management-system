using OU.CMS.Core.BusinessLogic.CompanyManagers.Tests.Commands;
using OU.CMS.Core.BusinessLogic.CompanyManagers.Tests.Queries;
using OU.CMS.Core.BusinessLogic.CompanyManagers.TestScores.Commands;
using OU.CMS.Models.Models.Common.Lookup;
using OU.CMS.Models.Models.Test;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace OU.CMS.Web.API.Controllers.CompanyManagers
{
    public class ManagerTestController : BaseSecureController
    {
        #region Test
        [HttpGet]
        public async Task<List<GetTestDto>> GetTestsAsCompanyManager()
        {
            return await new GetTestsAsCompanyManagerQuery().GetTestsAsCompanyManager(UserInfo);
        }

        public async Task<List<LookupDto<Guid>>> GetTestsAsCompanyManagerForLookup()
        {
            return await new GetTestsAsCompanyManagerForLookupQuery().GetTestsAsCompanyManagerForLookup(UserInfo);
        }

        [HttpGet]
        public async Task<GetTestDto> GetTestAsCompanyManager(Guid testId)
        {
            return await new GetTestAsCompanyManagerQuery(testId).GetTestAsCompanyManager(UserInfo);
        }

        [HttpPost]
        public async Task<GetTestDto> CreateTest(CreateTestDto dto)
        {
            return await new CreateTestCommand(dto).CreateTest(UserInfo);
        }

        [HttpPost]
        public async Task<GetTestDto> UpdateTest(UpdateTestDto dto)
        {
            return await new UpdateTestCommand(dto).UpdateTest(UserInfo);
        }

        [HttpDelete]
        public async Task DeleteTest(Guid testId)
        {
            await new DeleteTestCommand(testId).DeleteTest(UserInfo);
        }
        #endregion

        #region TestScores
        [HttpPost]
        public async Task<TestScoreDto> CreateTestScore(CreateTestScoreDto dto)
        {
            return await new CreateTestScoreCommand(dto).CreateTestScore(UserInfo);
        }

        [HttpPost]
        public async Task<TestScoreDto> UpdateTestScore(UpdateTestScoreDto dto)
        {
            return await new UpdateTestScoreCommand(dto).UpdateTestScore(UserInfo);
        }

        [HttpDelete]
        public async Task DeleteTestScore(Guid testScoreId)
        {
            await new DeleteTestScoreCommand(testScoreId).DeleteTestScore(UserInfo);
        }
        #endregion
    }
}