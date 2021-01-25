using OU.CMS.Core.BusinessLogic.Common.Jobs.Commands;
using OU.CMS.Core.BusinessLogic.Common.Jobs.Queries;
using OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Commands;
using OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Queries;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.Candidate;
using OU.CMS.Models.Models.JobOpening;
using System;
using System.Collections.Generic;
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

        #region Candidate
        public async Task<GetCandidateDto> GetCandidate(Guid candidateId)
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            var candidate = (await new GetCandidatesInternalQuery(candidateId: candidateId, companyId: UserInfo.CompanyId).GetAllCandidates()).SingleOrDefault();
            if (candidate == null)
                throw new Exception("Candidate does not exist!");
            return candidate;
        }

        [HttpGet]
        public async Task<List<GetCandidateDto>> GetCandidatesForCompany()
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var candidates = await new GetCandidatesInternalQuery(companyId: UserInfo.CompanyId).GetAllCandidates();

                return candidates;
            }
        }

        [HttpGet]
        public async Task<List<GetCandidateDto>> GetCandidatesForJobOpening(Guid jobOpeningId)
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var candidates = await new GetCandidatesInternalQuery(companyId: UserInfo.CompanyId, jobOpeningId: jobOpeningId).GetAllCandidates();

                return candidates;
            }
        }

        [HttpGet]
        public async Task DeleteCandidate(Guid candidateId)
        {
            if (!UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                await new DeleteCandidateCommand(candidateId).DeleteCandidate(UserInfo);
            }
        }
        #endregion

        #region Candidate Tests
        [HttpGet]
        public async Task<List<CandidateTestDto>> GetCandidateTestsAsCompanyManager(Guid testId)
        {
            using (var db = new CMSContext())
            {
                if (UserInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                return (await new GetCandidateTestsInternalQuery(testId, companyId: UserInfo.CompanyId).GetCandidateTests());
            }
        }

        [HttpGet]
        public async Task<CandidateTestDto> GetCandidateTestAsCompanyManager(Guid candidateId, Guid testId)
        {
            using (var db = new CMSContext())
            {
                if (UserInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                return (await new GetCandidateTestsInternalQuery(testId, companyId: UserInfo.CompanyId, candidateId: candidateId).GetCandidateTests()).SingleOrDefault();
            }
        }

        [HttpPost]
        public async Task<CandidateTestDto> CreateCandidateTest(CreateCandidateTestDto dto)
        {
            return await new CreateCandidateTestCommand(dto).CreateCandidateTest(UserInfo);
        }

        [HttpPost]
        public async Task UpdateCandidateTestScore(UpdateCandidateTestScoreDto dto)
        {
            await new UpdateCandidateTestScoreCommand(dto).UpdateCandidateTestScore(UserInfo);
        }
        #endregion
    }
}
