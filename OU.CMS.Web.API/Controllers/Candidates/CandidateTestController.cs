using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.Test;
using OU.CMS.Models.Models.Common;
using OU.CMS.Domain.Entities;
using Microsoft.AspNet.Identity;
using OU.CMS.Models.Models.Company;
using OU.CMS.Core.BusinessLogic.Candidates.Tests.Queries;

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