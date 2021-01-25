using OU.CMS.Core.BusinessLogic.Candidates.Tests.Queries;
using OU.CMS.Models.Models.Test;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace OU.CMS.Web.API.Controllers.Candidates
{
    public class CandidateTestController : BaseSecureController
    {
        #region Test
        [HttpGet]
        public async Task<List<GetTestDto>> GetTestsAsCandidate()
        {
            return await new GetTestsAsCandidateQuery().GetTestsAsCandidate(UserInfo);
        }

        [HttpGet]
        public async Task<GetTestDto> GetTestAsCandidate(Guid testId)
        {
            return await new GetTestAsCandidateQuery(testId).GetTestAsCandidate(UserInfo);
        }
        #endregion
    }
}