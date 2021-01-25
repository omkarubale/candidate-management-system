using OU.CMS.Core.BusinessLogic.Candidates.Jobs.Commands;
using OU.CMS.Core.BusinessLogic.Candidates.Jobs.Queries;
using OU.CMS.Core.BusinessLogic.Common.Jobs.Commands;
using OU.CMS.Core.BusinessLogic.Common.Jobs.Queries;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.Candidate;
using OU.CMS.Models.Models.JobOpening;
using System;
using System.Collections.Generic;
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

        #region Candidate
        [HttpGet]
        public async Task<GetCandidateDto> GetCandidate(Guid candidateId)
        {

            var candidate = (await new GetCandidatesInternalQuery(candidateId: candidateId, userId: UserInfo.UserId).GetAllCandidates()).SingleOrDefault();
            if (candidate == null)
                throw new Exception("Candidate does not exist!");

            return candidate;
        }

        [HttpPost]
        public async Task<GetCandidateDto> CreateCandidate(CreateCandidateDto dto)
        {
            return await new CreateCandidateCommand(dto).CreateCandidate(UserInfo);
        }

        [HttpGet]
        public async Task<List<GetCandidateDto>> GetCandidatesForUser()
        {
            if (!UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var candidates = await new GetCandidatesInternalQuery(userId: UserInfo.UserId).GetAllCandidates();

                return candidates;
            }
        }

        [HttpDelete]
        public async Task DeleteCandidate(Guid candidateId)
        {
            if (!UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                await new DeleteCandidateCommand(candidateId).DeleteCandidate(UserInfo);
            }
        }

        [HttpGet]
        public async Task<CandidateTestDto> GetCandidateTestAsCandidate(Guid candidateId, Guid testId)
        {
            using (var db = new CMSContext())
            {
                if (!UserInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                return (await new GetCandidateTestsInternalQuery(testId, userId: UserInfo.UserId, candidateId: candidateId).GetCandidateTests()).SingleOrDefault();
            }
        }
        #endregion
    }
}
