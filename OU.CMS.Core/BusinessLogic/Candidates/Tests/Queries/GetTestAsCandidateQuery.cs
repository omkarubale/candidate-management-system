using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.Test;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.Candidates.Tests.Queries
{
    public class GetTestAsCandidateQuery : BaseQuery<GetTestsAsCandidateQuery>
    {
        public Guid TestId { get; set; }

        public GetTestAsCandidateQuery(Guid testId)
        {
            TestId = testId;
            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<GetTestAsCandidateQuery>
        {
            public Validator()
            {
                RuleFor(i => i.TestId).NotNull().NotEmpty();
            }
        }

        public async Task<GetTestDto> GetTestAsCandidate(UserInfo userInfo)
        {
            if (!userInfo.IsCandidateLogin)
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
                                  tst.Id == TestId &&
                                  cnd.UserId == userInfo.UserId
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
    }
}
