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

namespace OU.CMS.Web.API.Controllers
{
    public class TestController : BaseSecureController
    {
        #region Test
        [HttpGet]
        public async Task<IEnumerable<GetTestDto>> GetAllTestsAsCandidate()
        {
            if (!UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var tests = (from cnd in db.Candidates
                             join pst in db.CandidateTests on cnd.Id equals pst.CandidateId
                             join tst in db.Tests on pst.TestId equals tst.Id
                             join cmp in db.Companies on tst.CompanyId equals cmp.Id
                             join usr in db.Users on tst.CreatedBy equals usr.Id
                             where
                             cnd.UserId == UserInfo.UserId
                             select new GetTestDto
                             {
                                 Id = tst.Id,
                                 Title = tst.Title,
                                 Description = tst.Description,

                                 Company = new CompanySimpleDto
                                 {
                                     Id = tst.CompanyId,
                                     Name = cmp.Name,
                                 },

                                 CreatedDetails = new CreatedOnDto
                                 {
                                     UserId = usr.Id,
                                     FullName = usr.FullName,
                                     ShortName = usr.ShortName,
                                     CreatedOn = tst.CreatedOn
                                 }
                             });

                return await tests.ToListAsync();
            }
        }

        [HttpGet]
        public async Task<IEnumerable<GetTestDto>> GetAllTestsAsCompanyManager()
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var testsQuery = (from tst in db.Tests
                                  join cmp in db.Companies on tst.CompanyId equals cmp.Id
                                  join usr in db.Users on tst.CreatedBy equals usr.Id
                                  join cdt in db.CandidateTests on tst.Id equals cdt.TestId into candidateTestTemp
                                  from cdt in candidateTestTemp.DefaultIfEmpty()
                                  where
                                  tst.CompanyId == (Guid)UserInfo.CompanyId
                                  select new
                                  {
                                      Id = tst.Id,
                                      Title = tst.Title,
                                      Description = tst.Description,

                                      CompanyId = tst.CompanyId,
                                      CompanyName = cmp.Name,

                                      UserId = usr.Id,
                                      UserFullName = usr.FullName,
                                      UserShortName = usr.ShortName,
                                      UserCreatedOn = tst.CreatedOn,

                                      CandidateTestId = cdt != null ? (Guid?)cdt.Id : null
                                  });

                var tests = await testsQuery.GroupBy(t => new { t.Id, t.Title, t.Description, t.CompanyId, t.CompanyName, t.UserId, t.UserFullName, t.UserShortName, t.UserCreatedOn }).Select(t => new GetTestDto
                {
                    Id = t.Key.Id,
                    Title = t.Key.Title,
                    Description = t.Key.Description,

                    Company = new CompanySimpleDto
                    {
                        Id = t.Key.CompanyId,
                        Name = t.Key.CompanyName,
                    },

                    CreatedDetails = new CreatedOnDto
                    {
                        UserId = t.Key.UserId,
                        FullName = t.Key.UserFullName,
                        ShortName = t.Key.UserShortName,
                        CreatedOn = t.Key.UserCreatedOn
                    },
                    TakersCount = t.Count(ct => ct.CandidateTestId != null)
                }).ToListAsync();

                return tests;
            }
        }

        [HttpGet]
        public async Task<GetTestDto> GetTestAsCandidate(Guid testId)
        {
            if (!UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var test = await (from cnd in db.Candidates
                                  join pst in db.CandidateTests on cnd.Id equals pst.CandidateId
                                  join tst in db.Tests on pst.TestId equals tst.Id
                                  join cmp in db.Companies on tst.CompanyId equals cmp.Id
                                  join tstc in db.TestScores on tst.Id equals tstc.TestId
                                  join usr in db.Users on tst.CreatedBy equals usr.Id
                                  where
                                  tst.Id == testId &&
                                  cnd.UserId == UserInfo.UserId
                                  group tstc by new { tst.Id, tst.Title, tst.Description, tst.CompanyId, CompanyName = cmp.Name, UserId = usr.Id, usr.FullName, usr.ShortName, tst.CreatedOn } into g
                                  select new GetTestDto
                                  {
                                      Id = g.Key.Id,
                                      Title = g.Key.Title,
                                      Description = g.Key.Description,

                                      Company = new CompanySimpleDto
                                      {
                                          Id = g.Key.CompanyId,
                                          Name = g.Key.CompanyName,
                                      },

                                      TestScores = g.Select(t => new TestScoreDto
                                      {
                                          Id = t.Id,
                                          Title = t.Title,
                                          IsMandatory = t.IsMandatory,
                                          MinimumScore = t.MinimumScore,
                                          MaximumScore = t.MaximumScore,
                                          CutoffScore = t.CutoffScore
                                      }).ToList(),

                                      CreatedDetails = new CreatedOnDto
                                      {
                                          UserId = g.Key.Id,
                                          FullName = g.Key.FullName,
                                          ShortName = g.Key.ShortName,
                                          CreatedOn = g.Key.CreatedOn
                                      }
                                  }).SingleOrDefaultAsync();

                if (test == null)
                    throw new Exception("Test does not exist!");

                return test;
            }
        }

        [HttpGet]
        public async Task<GetTestDto> GetTestAsCompanyManager(Guid testId)
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var test = await (from tst in db.Tests
                                  join cmp in db.Companies on tst.CompanyId equals cmp.Id
                                  join usr in db.Users on tst.CreatedBy equals usr.Id
                                  where tst.Id == testId &&
                                  tst.CompanyId == (Guid)UserInfo.CompanyId
                                  select new GetTestDto
                                  {
                                      Id = tst.Id,
                                      Title = tst.Title,
                                      Description = tst.Description,

                                      Company = new CompanySimpleDto
                                      {
                                          Id = cmp.Id,
                                          Name = cmp.Name,
                                      },

                                      CreatedDetails = new CreatedOnDto
                                      {
                                          UserId = usr.Id,
                                          FullName = usr.FullName,
                                          ShortName = usr.ShortName,
                                          CreatedOn = tst.CreatedOn
                                      }
                                  }).SingleOrDefaultAsync();

                var testScores = await (from tstc in db.TestScores
                                        where
                                        tstc.TestId == testId
                                        select new TestScoreDto
                                        {
                                            Id = tstc.Id,
                                            Title = tstc.Title,
                                            IsMandatory = tstc.IsMandatory,
                                            MinimumScore = tstc.MinimumScore,
                                            MaximumScore = tstc.MaximumScore,
                                            CutoffScore = tstc.CutoffScore
                                        }).ToListAsync();

                if (test == null)
                    throw new Exception("Test does not exist!");

                test.TestScores = testScores;

                return test;
            }
        }

        [HttpPost]
        public async Task<GetTestDto> CreateTest(CreateTestDto dto)
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var checkExistingTest = db.Tests.Any(c => c.Title == dto.Title.Trim() && c.CompanyId == UserInfo.CompanyId);
                if (checkExistingTest)
                    throw new Exception("Test with this name already exists!");

                var test = new Test
                {
                    Id = Guid.NewGuid(),
                    Title = dto.Title.Trim(),
                    Description = dto.Description.Trim(),
                    CompanyId = (Guid)UserInfo.CompanyId,
                    CreatedBy = UserInfo.UserId,
                    CreatedOn = DateTime.UtcNow,
                };

                db.Tests.Add(test);

                await db.SaveChangesAsync();

                return await GetTestAsCompanyManager(test.Id);
            }
        }

        [HttpPost]
        public async Task<GetTestDto> UpdateTest(UpdateTestDto dto)
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var test = await db.Tests.SingleOrDefaultAsync(c => c.Id == dto.Id && c.CompanyId == UserInfo.CompanyId);
                if (test == null)
                    throw new Exception("Test with Id not found!");

                var checkExistingTest = db.Tests.Any(c => c.Title == dto.Title.Trim() && c.CompanyId == UserInfo.CompanyId && c.Id != dto.Id);
                if (checkExistingTest)
                    throw new Exception("Test with this title already exists!");

                test.Title = dto.Title;
                test.Description = dto.Description;

                await db.SaveChangesAsync();

                return await GetTestAsCompanyManager(test.Id);
            }
        }

        [HttpDelete]
        public async Task DeleteTest(Guid testId)
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var test = await db.Tests.SingleOrDefaultAsync(c => c.Id == testId && c.CompanyId == UserInfo.CompanyId);

                if (test == null)
                    throw new Exception("Test with Id not found!");

                db.CandidateTestScores.RemoveRange(test.CandidateTests.SelectMany(cd => cd.CandidateTestScores));
                db.CandidateTests.RemoveRange(test.CandidateTests);
                db.TestScores.RemoveRange(test.TestScores);
                db.Tests.Remove(test);

                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region TestScores
        [HttpPost]
        public async Task<TestScoreDto> CreateTestScore(CreateTestScoreDto dto)
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var checkExistingTest = db.TestScores.Include(ts => ts.Test).Any(ts => ts.Title == dto.Title.Trim() && ts.TestId == dto.TestId && ts.Test.CompanyId == UserInfo.CompanyId);
                if (checkExistingTest)
                    throw new Exception("Test Score with this title already exists in this Test!");

                var testScore = new TestScore
                {
                    Id = Guid.NewGuid(),
                    TestId = dto.TestId,
                    Title = dto.Title.Trim(),
                    IsMandatory = dto.IsMandatory,
                    MinimumScore = dto.MinimumScore,
                    MaximumScore = dto.MaximumScore,
                    CutoffScore = dto.CutoffScore
                };

                db.TestScores.Add(testScore);

                await db.SaveChangesAsync();

                return new TestScoreDto()
                {
                    Id = testScore.Id,
                    Title = testScore.Title,
                    IsMandatory = testScore.IsMandatory,
                    MinimumScore = testScore.MinimumScore,
                    MaximumScore = testScore.MaximumScore,
                    CutoffScore = testScore.CutoffScore
                };
            }
        }

        [HttpPost]
        public async Task<TestScoreDto> UpdateTestScore(UpdateTestScoreDto dto)
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var testScore = await db.TestScores.Include(ts => ts.Test).SingleOrDefaultAsync(ts => ts.Id == dto.Id && ts.Test.CompanyId == UserInfo.CompanyId);
                if (testScore == null)
                    throw new Exception("Test with Id not found!");

                var checkExistingTestScore = db.TestScores.Include(ts => ts.Test).Any(ts => ts.Title == dto.Title.Trim() && ts.TestId == dto.TestId && ts.Test.CompanyId == UserInfo.CompanyId && ts.Id != dto.Id);
                if (checkExistingTestScore)
                    throw new Exception("Test with this title already exists!");

                testScore.Title = dto.Title;
                testScore.IsMandatory = dto.IsMandatory;
                testScore.MinimumScore = dto.MinimumScore;
                testScore.MaximumScore = dto.MaximumScore;
                testScore.CutoffScore = dto.CutoffScore;

                await db.SaveChangesAsync();

                return new TestScoreDto()
                {
                    Id = testScore.Id,
                    Title = testScore.Title,
                    IsMandatory = testScore.IsMandatory,
                    MinimumScore = testScore.MinimumScore,
                    MaximumScore = testScore.MaximumScore,
                    CutoffScore = testScore.CutoffScore
                };
            }
        }

        [HttpDelete]
        public async Task DeleteTestScore(Guid testScoreId)
        {
            if (UserInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var testScore = await db.TestScores.Include(ts => ts.Test).SingleOrDefaultAsync(ts => ts.Id == testScoreId && ts.Test.CompanyId == UserInfo.CompanyId);
                if (testScore == null)
                    throw new Exception("Test with Id not found!");

                db.TestScores.Remove(testScore);

                await db.SaveChangesAsync();

                return;
            }
        }
        #endregion
    }
}
