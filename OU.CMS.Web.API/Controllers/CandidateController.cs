using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.Candidate;
using OU.CMS.Models.Models.Common;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Models.User;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.JobOpening;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OU.Utility.Extensions;

namespace OU.CMS.Web.API.Controllers
{
    public class CandidateController : BaseSecureController
    {
        #region Candidate
        private async Task<List<GetCandidateDto>> GetAllCandidates(Guid? candidateId = null, Guid? companyId = null, Guid? jobOpeningId = null, Guid? userId = null)
        {
            using (var db = new CMSContext())
            {
                var isCandidateFilter = candidateId != null && candidateId != Guid.Empty;
                var isCompanyFilter = companyId != null && companyId != Guid.Empty;
                var isJobOpeningFilter = jobOpeningId != null && jobOpeningId != Guid.Empty;
                var isUserFilter = userId != null && userId != Guid.Empty;

                var candidatesQuery = from cnd in db.Candidates
                                      join usr in db.Users on cnd.UserId equals usr.Id
                                      join cmp in db.Companies on cnd.CompanyId equals cmp.Id
                                      join jo in db.JobOpenings on cnd.JobOpeningId equals jo.Id
                                      join lc in db.Users on cnd.CreatedBy equals lc.Id
                                      select new
                                      {
                                          CandidateId = cnd.Id,

                                          UserId = usr.Id,
                                          UserFullName = usr.FullName,
                                          UserShortName = usr.ShortName,
                                          UserEmail = usr.Email,

                                          CompanyId = cmp.Id,
                                          CompanyName = cmp.Name,

                                          JobOpeningId = jo.Id,
                                          JobOpeningTitle = jo.Title,
                                          JobOpeningDescription = jo.Description,

                                          CreatedDetailsUserId = lc.Id,
                                          CreatedDetailsFullName = lc.FullName,
                                          CreatedDetailsShortName = lc.ShortName,
                                          CreatedDetailsCreatedOn = cnd.CreatedOn
                                      };

                if (isCandidateFilter)
                    candidatesQuery = candidatesQuery.Where(c => c.CandidateId == candidateId);

                if (isCompanyFilter)
                    candidatesQuery = candidatesQuery.Where(c => c.CompanyId == companyId);

                if (isJobOpeningFilter)
                    candidatesQuery = candidatesQuery.Where(c => c.JobOpeningId == jobOpeningId);

                if (isUserFilter)
                    candidatesQuery = candidatesQuery.Where(c => c.UserId == userId);

                var candidates = await candidatesQuery.Select(c => new GetCandidateDto
                {
                    CandidateId = c.CandidateId,
                    User = new UserSimpleDto
                    {
                        UserId = c.UserId,
                        FullName = c.UserFullName,
                        ShortName = c.UserShortName,
                        Email = c.UserEmail
                    },
                    Company = new CompanySimpleDto
                    {
                        Id = c.CompanyId,
                        Name = c.CompanyName
                    },
                    JobOpening = new JobOpeningSimpleDto
                    {
                        Id = c.JobOpeningId,
                        Title = c.JobOpeningTitle,
                        Description = c.JobOpeningDescription,
                    },
                    CreatedDetails = new CreatedOnDto
                    {
                        UserId = c.CreatedDetailsUserId,
                        FullName = c.CreatedDetailsFullName,
                        ShortName = c.CreatedDetailsShortName,
                        CreatedOn = c.CreatedDetailsCreatedOn
                    }
                }).ToListAsync();

                return candidates;
            }
        }

        [HttpGet]
        public async Task<GetCandidateDto> GetCandidate(Guid candidateId)
        {
            using (var db = new CMSContext())
            {
                var candidate = (await GetAllCandidates(candidateId: candidateId, companyId: !UserInfo.IsCandidateLogin ? UserInfo.CompanyId : null, userId: UserInfo.IsCandidateLogin ? (Guid?)UserInfo.UserId : null)).SingleOrDefault();

                if (candidate == null)
                    throw new Exception("Candidate does not exist!");

                return candidate;
            }
        }

        [HttpGet]
        public async Task<List<GetCandidateDto>> GetCandidatesForCompany(Guid companyId)
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var candidates = await GetAllCandidates(companyId: companyId);

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
                var candidates = await GetAllCandidates(jobOpeningId: jobOpeningId);

                return candidates;
            }
        }

        [HttpGet]
        public async Task<List<GetCandidateDto>> GetCandidatesForUser(Guid userId)
        {
            if (!UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var candidates = await GetAllCandidates(userId: userId);

                return candidates;
            }
        }

        [HttpPost]
        public async Task<GetCandidateDto> CreateCandidate(CreateCandidateDto dto)
        {
            if (!UserInfo.IsCandidateLogin || UserInfo.UserId != dto.UserId)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var checkExistingCandidate = db.Candidates.Any(c => c.UserId == dto.UserId && c.JobOpeningId == dto.JobOpeningId);
                if (checkExistingCandidate)
                    throw new Exception("Candidate has already applied for this Job Position!");

                var candidate = new Candidate
                {
                    Id = Guid.NewGuid(),
                    UserId = dto.UserId,
                    CompanyId = dto.CompanyId,
                    JobOpeningId = dto.JobOpeningId,
                    CreatedBy = UserInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                db.Candidates.Add(candidate);

                await db.SaveChangesAsync();

                return await GetCandidate(candidate.Id);
            }
        }

        [HttpDelete]
        public async Task DeleteCandidate(Guid candidateId)
        {
            using (var db = new CMSContext())
            {
                var candidate = await db.Candidates.SingleOrDefaultAsync(c => c.Id == candidateId);

                if ((!UserInfo.IsCandidateLogin && UserInfo.CompanyId != candidate.CompanyId) || (UserInfo.IsCandidateLogin && UserInfo.UserId != candidate.UserId))
                    throw new Exception("You do not have access to perform this action!");

                if (candidate == null)
                    throw new Exception("Candidate with Id not found!");

                var candidateTests = await db.CandidateTests
                    .Include(ct => ct.CandidateTestScores)
                    .Where(ct => ct.CandidateId == candidateId).ToListAsync();

                db.CandidateTestScores.RemoveRange(candidateTests.SelectMany(ct => ct.CandidateTestScores));
                db.CandidateTests.RemoveRange(candidateTests);
                db.Candidates.Remove(candidate);

                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region CandidateTests
        [HttpPost]
        public async Task<CandidateTestDto> CreateCandidateTest(CreateCandidateTestDto dto)
        {
            using (var db = new CMSContext())
            {
                var candidate = await db.Candidates.SingleOrDefaultAsync(c => c.Id == dto.CandidateId);
                if (UserInfo.IsCandidateLogin || UserInfo.CompanyId != candidate.CompanyId)
                    throw new Exception("You do not have access to perform this action!");

                var checkExistingCandidate = db.CandidateTests.Any(c => c.CandidateId == dto.CandidateId && c.TestId == dto.TestId);
                if (checkExistingCandidate)
                    throw new Exception("Candidate already has this Test in his profile!");

                var candidateTest = new CandidateTest
                {
                    Id = Guid.NewGuid(),
                    CandidateId = dto.CandidateId,
                    TestId = dto.TestId,
                };

                db.CandidateTests.Add(candidateTest);

                var testScores = await db.TestScores.Where(c => c.TestId == dto.TestId).ToListAsync();

                foreach (var testScore in testScores)
                {
                    var candidateTestScore = new CandidateTestScore
                    {
                        Id = Guid.NewGuid(),
                        CandidateTestId = candidateTest.Id,
                        TestScoreId = testScore.Id,
                        Value = null,
                        Comment = null
                    };

                    db.CandidateTestScores.Add(candidateTestScore);
                }

                await db.SaveChangesAsync();

                return await GetCandidateTestAsCompanyManager(candidateTest.CandidateId, candidateTest.TestId);
            }
        }

        private async Task<List<CandidateTestDto>> GetCandidatesForTest(CMSContext db, Guid testId, Guid? candidateId = null)
        {
            var isCandidateFilter = candidateId != null && candidateId != Guid.Empty;

            var candidates = await (from cdt in db.CandidateTests
                                    join cnd in db.Candidates on cdt.CandidateId equals cnd.Id
                                    join usr in db.Users on cnd.UserId equals usr.Id
                                    join cmp in db.Companies on cnd.CompanyId equals cmp.Id
                                    join jo in db.JobOpenings on cnd.JobOpeningId equals jo.Id
                                    join lc in db.Users on cnd.CreatedBy equals lc.Id
                                    where
                                    cdt.TestId == testId &&
                                    (!isCandidateFilter || cnd.Id == candidateId)
                                    select new CandidateTestDto
                                    {
                                        Candidate = new GetCandidateDto
                                        {
                                            CandidateId = cnd.Id,
                                            User = new UserSimpleDto
                                            {
                                                UserId = usr.Id,
                                                FullName = usr.FullName,
                                                ShortName = usr.ShortName,
                                                Email = usr.Email
                                            },
                                            Company = new CompanySimpleDto
                                            {
                                                Id = cmp.Id,
                                                Name = cmp.Name
                                            },
                                            JobOpening = new JobOpeningSimpleDto
                                            {
                                                Id = jo.Id,
                                                Title = jo.Title,
                                                Description = jo.Description,
                                            },
                                            CreatedDetails = new CreatedOnDto
                                            {
                                                UserId = lc.Id,
                                                FullName = lc.FullName,
                                                ShortName = lc.ShortName,
                                                CreatedOn = cnd.CreatedOn
                                            }
                                        }
                                    }).ToListAsync();

            var candidateTestsScores = (await (from cdt in db.CandidateTests
                                               join tst in db.Tests on cdt.TestId equals tst.Id
                                               join cdts in db.CandidateTestScores on cdt.Id equals cdts.CandidateTestId
                                               join tsts in db.TestScores on cdts.TestScoreId equals tsts.Id
                                               where
                                               cdt.TestId == testId &&
                                               (!isCandidateFilter || cdt.CandidateId == candidateId)
                                               select new
                                               {
                                                   CandidateId = cdt.CandidateId,
                                                   TestTitle = tst.Title,

                                                   TestScoreId = tsts.Id,
                                                   TestScoreTitle = tsts.Title,
                                                   TestScoreIsMandatory = tsts.IsMandatory,

                                                   CandidateTestScoreId = cdts.Id,
                                                   CandidateTestScoreValue = cdts.Value,
                                                   CandidateTestScoreComment = cdts.Comment
                                               }).ToListAsync())
                                        .GroupBy(t => new { t.CandidateId, t.TestTitle })
                                        .Select(t => new
                                        {
                                            CandidateId = t.Key.CandidateId,
                                            Title = t.Key.TestTitle,

                                            CandidateTestScores = t.Select(ts => new CandidateTestScoreDto
                                            {
                                                CandidateTestScoreId = ts.CandidateTestScoreId,
                                                TestScoreId = ts.TestScoreId,
                                                Title = ts.TestScoreTitle,
                                                IsMandatory = ts.TestScoreIsMandatory,

                                                Value = ts.CandidateTestScoreValue,
                                                Comment = ts.CandidateTestScoreComment
                                            }).OrderBy(ts => ts.Title).ToList()
                                        }).ToList();

            var sortedTotalScores = candidateTestsScores.Select(cdsts => (double?)cdsts.CandidateTestScores.Select(cdts => cdts.Value)?.Sum() ?? 0d).ToList();
            sortedTotalScores.Sort();

            foreach (var candidate in candidates)
            {
                var candidateTestsScore = candidateTestsScores.SingleOrDefault(cts => cts.CandidateId == candidate.Candidate.CandidateId);
                candidate.Title = candidateTestsScore.Title;
                candidate.CandidateTestScores = candidateTestsScore.CandidateTestScores;
                candidate.TotalScore = candidateTestsScore.CandidateTestScores.Select(ts => ts.Value)?.Sum() ?? 0;
                candidate.Percentile = (decimal)sortedTotalScores.GetPercentile((double)candidate.TotalScore);
            }

            return candidates;
        }

        [HttpGet]
        public async Task<List<CandidateTestDto>> GetCandidateTestsAsCompanyManager(Guid testId)
        {
            using (var db = new CMSContext())
            {
                var test = await db.Tests.SingleOrDefaultAsync(t => t.Id == testId);
                if (UserInfo.IsCandidateLogin || UserInfo.CompanyId != test.CompanyId)
                    throw new Exception("You do not have access to perform this action!");

                return await GetCandidatesForTest(db, testId);
            }
        }

        [HttpGet]
        public async Task<CandidateTestDto> GetCandidateTestAsCompanyManager(Guid candidateId, Guid testId)
        {
            using (var db = new CMSContext())
            {
                var candidate = await db.Candidates.SingleOrDefaultAsync(c => c.Id == candidateId);
                if (UserInfo.IsCandidateLogin || UserInfo.CompanyId != candidate.CompanyId)
                    throw new Exception("You do not have access to perform this action!");

                return (await GetCandidatesForTest(db, testId, candidateId)).SingleOrDefault();
            }
        }

        [HttpGet]
        public async Task<CandidateTestDto> GetCandidateTestAsCandidate(Guid candidateId, Guid testId)
        {
            using (var db = new CMSContext())
            {
                var candidate = await db.Candidates.SingleOrDefaultAsync(c => c.Id == candidateId);
                if (!UserInfo.IsCandidateLogin || UserInfo.UserId != candidate.UserId)
                    throw new Exception("You do not have access to perform this action!");

                return (await GetCandidatesForTest(db, testId, candidateId)).SingleOrDefault();
            }
        }

        [HttpPost]
        public async Task UpdateCandidateTestScore(UpdateCandidateTestScoreDto dto)
        {
            using (var db = new CMSContext())
            {
                var candidateTestScore = await db.CandidateTestScores
                    .Include(cts => cts.TestScore)
                    .Include(cts => cts.CandidateTest)
                    .SingleOrDefaultAsync(ts => ts.Id == dto.CandidateTestScoreId);

                var candidate = await db.Candidates.SingleOrDefaultAsync(c => c.Id == candidateTestScore.CandidateTest.CandidateId);
                if (UserInfo.IsCandidateLogin || UserInfo.CompanyId != candidate.CompanyId)
                    throw new Exception("You do not have access to perform this action!");

                if (candidateTestScore == null)
                    throw new Exception("Candidate Test Score doesn't exist!");

                if (candidateTestScore.TestScore.MinimumScore > dto.Value)
                    throw new Exception("Test Score has to be more than Minimum Value!");

                if (candidateTestScore.TestScore.MaximumScore < dto.Value)
                    throw new Exception("Test Score has to be less than Maximum Value!");

                candidateTestScore.Value = dto.Value;
                candidateTestScore.Comment = dto.Comment;

                await db.SaveChangesAsync();
            }
        }
        #endregion
    }
}
