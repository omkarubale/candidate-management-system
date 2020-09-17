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

namespace OU.CMS.Web.API.Controllers
{
    public class TestController : ApiController
    {
        private Guid myUserId = new Guid("1ff58b86-28a7-4324-bc40-518c29135f86");
        //private string myEmail = "oubale@gmail.com";

        #region Test
        public async Task<IEnumerable<GetTestDto>> GetAllTests()
        {
            using (var db = new CMSContext())
            {
                var tests = await (from tst in db.Tests
                                   join usr in db.Users on tst.CreatedBy equals usr.Id
                                   select new GetTestDto
                                   {
                                       Id = tst.Id,
                                       Title = tst.Title,
                                       CreatedDetails = new CreatedOnDto
                                       {
                                           UserId = usr.Id,
                                           FullName = usr.FullName,
                                           ShortName = usr.ShortName,
                                           CreatedOn = tst.CreatedOn
                                       }
                                   }).ToListAsync();

                return tests;
            }
        }

        public async Task<GetTestDto> GetTest(Guid id)
        {
            using (var db = new CMSContext())
            {
                var test = await (from tst in db.Tests
                                  join tstc in db.TestScores on tst.Id equals tstc.TestId
                                  join usr in db.Users on tst.CreatedBy equals usr.Id
                                  where tst.Id == id
                                  group tstc by new { tst.Id, tst.Title, UserId = usr.Id, usr.FullName, usr.ShortName, tst.CreatedOn } into g
                                  select new GetTestDto
                                  {
                                      Id = g.Key.Id,
                                      Title = g.Key.Title,

                                      TestScores = g.Select(t => new TestScoreDto
                                      {
                                          Id = t.Id,
                                          Title = t.Title,
                                          IsMandatory = t.IsMandatory
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

        public async Task<GetTestDto> CreateTest(CreateTestDto dto)
        {
            using (var db = new CMSContext())
            {
                var checkExistingTest = db.Tests.Any(c => c.Title == dto.Title.Trim());
                if (checkExistingTest)
                    throw new Exception("Test with this name already exists!");

                var test = new Test
                {
                    Id = Guid.NewGuid(),
                    Title = dto.Title.Trim(),
                    CreatedBy = myUserId, //TODO: Change to identityUser.UserId
                    CreatedOn = DateTime.UtcNow
                };

                db.Tests.Add(test);

                await db.SaveChangesAsync();

                return await GetTest(test.Id);
            }
        }

        public async Task<GetTestDto> UpdateTest(UpdateTestDto dto)
        {
            using (var db = new CMSContext())
            {
                var test = await db.Tests.SingleOrDefaultAsync(c => c.Id == dto.Id);
                if (test == null)
                    throw new Exception("Test with Id not found!");

                var checkExistingTest = db.Tests.Any(c => c.Title == dto.Title.Trim() && c.Id != dto.Id);
                if (checkExistingTest)
                    throw new Exception("Test with this title already exists!");

                test.Title = dto.Title;

                await db.SaveChangesAsync();

                return await GetTest(test.Id);
            }
        }

        public async Task DeleteTest(Guid id)
        {
            using (var db = new CMSContext())
            {
                var company = await db.Tests.SingleOrDefaultAsync(c => c.Id == id);

                if (company == null)
                    throw new Exception("Test with Id not found!");

                db.Tests.Remove(company);

                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region TestScores
        public async Task<TestScoreDto> CreateTestScore(CreateTestScoreDto dto)
        {
            using (var db = new CMSContext())
            {
                var checkExistingTest = db.TestScores.Any(t => t.Title == dto.Title.Trim() && t.TestId == dto.TestId);
                if (checkExistingTest)
                    throw new Exception("Test Score with this title already exists in this Test!");

                var testScore = new TestScore
                {
                    Id = Guid.NewGuid(),
                    TestId = dto.TestId,
                    Title = dto.Title.Trim(),
                    IsMandatory = dto.IsMandatory,
                };

                db.TestScores.Add(testScore);

                await db.SaveChangesAsync();

                return new TestScoreDto()
                {
                    Id = testScore.Id,
                    Title = testScore.Title,
                    IsMandatory = testScore.IsMandatory
                };
            }
        }

        public async Task<TestScoreDto> UpdateTestScore(UpdateTestScoreDto dto)
        {
            using (var db = new CMSContext())
            {
                var testScore = await db.TestScores.SingleOrDefaultAsync(c => c.Id == dto.Id);
                if (testScore == null)
                    throw new Exception("Test with Id not found!");

                var checkExistingTestScore = db.TestScores.Any(t => t.Title == dto.Title.Trim() && t.TestId == dto.TestId && t.Id != dto.Id);
                if (checkExistingTestScore)
                    throw new Exception("Test with this title already exists!");

                testScore.Title = dto.Title;
                testScore.IsMandatory = dto.IsMandatory;

                await db.SaveChangesAsync();

                return new TestScoreDto()
                {
                    Id = testScore.Id,
                    Title = testScore.Title,
                    IsMandatory = testScore.IsMandatory
                };
            }
        }
        #endregion
    }
}
