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

namespace OU.CMS.Web.API.Controllers
{
    public class CandidateController : ApiController
    {
        private Guid myUserId = new Guid("1ff58b86-28a7-4324-bc40-518c29135f86");

        #region Candidate
        private async Task<List<GetCandidateDto>> GetAllCandidates(Guid? candidateId = null, Guid? companyId = null, Guid? jobOpeningId = null, Guid? userId = null)
        {
            using (var db = new CMSContext())
            {
                var isCandidateFilter = candidateId != null && candidateId != Guid.Empty;
                var isCompanyFilter = companyId != null && companyId != Guid.Empty;
                var isJobOpeningFilter = jobOpeningId != null && jobOpeningId != Guid.Empty;
                var isUserFilter = userId != null && userId != Guid.Empty;

                var candidates = (from cnd in db.Candidates
                                  join usr in db.Users on cnd.UserId equals usr.Id
                                  join cmp in db.Companies on cnd.CompanyId equals cmp.Id
                                  join jo in db.JobOpenings on cnd.JobOpeningId equals jo.Id
                                  join lc in db.Users on cnd.CreatedBy equals lc.Id
                                  where
                                  (!isCandidateFilter || cnd.Id == candidateId) &&
                                  (!isCompanyFilter || cnd.CompanyId == companyId) &&
                                  (!isJobOpeningFilter || cnd.JobOpeningId == jobOpeningId) &&
                                  (!isUserFilter || cnd.UserId == userId)
                                  select new GetCandidateDto
                                  {
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
                                  });

                return await candidates.ToListAsync();
            }
        }

        public async Task<GetCandidateDto> GetCandidate(Guid candidateId)
        {
            using (var db = new CMSContext())
            {
                var candidate = (await GetAllCandidates(candidateId: candidateId)).SingleOrDefault();

                if (candidate == null)
                    throw new Exception("Candidate does not exist!");

                return candidate;
            }
        }

        public async Task<List<GetCandidateDto>> GetCandidatesForCompany(Guid companyId)
        {
            using (var db = new CMSContext())
            {
                var candidates = await GetAllCandidates(companyId: companyId);

                return candidates;
            }
        }

        public async Task<List<GetCandidateDto>> GetCandidatesForJobOpening(Guid jobOpeningId)
        {
            using (var db = new CMSContext())
            {
                var candidates = await GetAllCandidates(jobOpeningId: jobOpeningId);

                return candidates;
            }
        }

        public async Task<List<GetCandidateDto>> GetCandidatesForUser(Guid userId)
        {
            using (var db = new CMSContext())
            {
                var candidates = await GetAllCandidates(userId: userId);

                return candidates;
            }
        }

        public async Task<GetCandidateDto> CreateCandidate(CreateCandidateDto dto)
        {
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
                    CreatedBy = myUserId, //TODO: Change to identityUser.UserId
                    CreatedOn = DateTime.UtcNow
                };

                db.Candidates.Add(candidate);

                await db.SaveChangesAsync();

                return await GetCandidate(candidate.Id);
            }
        }

        public async Task DeleteCandidate(Guid candidateId)
        {
            using (var db = new CMSContext())
            {
                var candidate = await db.Candidates.SingleOrDefaultAsync(c => c.Id == candidateId);

                if (candidate == null)
                    throw new Exception("Candidate with Id not found!");

                var candidateTests = await db.CandidateTests.Include(ct => ct.CandidateTestScores).Where(ct => ct.CandidateId == candidateId).ToListAsync();

                db.CandidateTestScores.RemoveRange(candidateTests.SelectMany(ct => ct.CandidateTestScores));
                db.CandidateTests.RemoveRange(candidateTests);
                db.Candidates.Remove(candidate);

                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region CandidateTests
        public async Task<CandidateTestDto> CreateCandidateTest(CreateCandidateTestDto dto)
        {
            using (var db = new CMSContext())
            {
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

                return await GetCandidateTest(candidateTest.CandidateId, candidateTest.TestId);
            }
        }

        public async Task<CandidateTestDto> GetCandidateTest(Guid candidateId, Guid testId)
        {
            using (var db = new CMSContext())
            {
                var candidateTest = (from cnd in db.Candidates
                                     join cdt in db.CandidateTests on cnd.Id equals cdt.CandidateId
                                     join tst in db.Tests on cdt.TestId equals tst.Id
                                     join cdts in db.CandidateTestScores on cdt.Id equals cdts.CandidateTestId
                                     join tsts in db.TestScores on cdts.TestScoreId equals tsts.Id
                                     where
                                     cnd.Id == candidateId &&
                                     tst.Id == testId
                                     select new
                                     {
                                         CandidateId = cnd.Id,
                                         TestTitle = tst.Title,

                                         TestScoreId = tsts.Id,
                                         TestScoreTitle = tsts.Title,
                                         TestScoreIsMandatory = tsts.IsMandatory,

                                         CandidateTestScoreId = cdts.Id,
                                         CandidateTestScoreValue = cdts.Value,
                                         CandidateTestScoreComment = cdts.Comment
                                     })
                                    .GroupBy(t => new { t.CandidateId, t.TestTitle })
                                    .Select(t => new CandidateTestDto
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
                                        }).ToList()
                                    });

                return await candidateTest.SingleOrDefaultAsync();
            }
        }

        public async Task UpdateCandidateTestScore(UpdateCandidateTestScoreDto dto)
        {
            using (var db = new CMSContext())
            {
                var candidateTestScore = await db.CandidateTestScores.Include(cts => cts.TestScore).SingleOrDefaultAsync(ts => ts.Id == dto.CandidateTestScoreId);

                if (candidateTestScore == null)
                    throw new Exception("Candidate Test Score doesn't exist!");

                if(candidateTestScore.TestScore.MinimumScore > dto.Value)
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
