using OU.CMS.Core.BusinessLogic.Candidates.Jobs.Queries;
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

namespace OU.CMS.Web.API.Controllers.Candidates
{
    public class CandidateJobController : BaseSecureController
    {
        #region Job Opening for Candidate
        [HttpGet]
        public async Task<List<GetCandidateJobOpeningDto>> GetAllJobOpeningsForCandidate()
        {
            return await new GetJobOpeningsForCandidateQuery(null).GetJobOpeningsForCandidate(UserInfo);
        }

        [HttpGet]
        public async Task<GetCandidateJobOpeningDto> GetJobOpeningForCandidate(Guid jobOpeningId)
        {
            return (await new GetJobOpeningsForCandidateQuery(jobOpeningId).GetJobOpeningsForCandidate(UserInfo)).SingleOrDefault();
        }
        #endregion
    }
}
