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

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Tests.Queries
{
    public class GetTestAsCompanyManagerQuery : BaseQuery<GetTestAsCompanyManagerQuery>
    {
        public Guid TestId { get; set; }

        public GetTestAsCompanyManagerQuery(Guid testId)
        {
            TestId = testId;
            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<GetTestAsCompanyManagerQuery>
        {
            public Validator()
            {
                RuleFor(i => i.TestId).NotNull().NotEmpty();
            }
        }

        public async Task<GetTestDto> GetTestAsCompanyManager(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var test = await (from tst in db.Tests
                                  join cmp in db.Companies on tst.CompanyId equals cmp.Id
                                  join cm in db.CompanyManagements on cmp.Id equals cm.CompanyId
                                  join usr in db.Users on tst.CreatedBy equals usr.Id
                                  where
                                  tst.Id == TestId &&
                                  tst.CompanyId == (Guid)userInfo.CompanyId &&
                                  cm.UserId == userInfo.UserId
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

                if (test == null)
                    throw new Exception("Test does not exist!");

                var testScores = await (from tstc in db.TestScores
                                        where
                                        tstc.TestId == TestId
                                        select new TestScoreDto
                                        {
                                            Id = tstc.Id,
                                            Title = tstc.Title,
                                            IsMandatory = tstc.IsMandatory,
                                            MinimumScore = tstc.MinimumScore,
                                            MaximumScore = tstc.MaximumScore,
                                            CutoffScore = tstc.CutoffScore
                                        }).ToListAsync();

                test.TestScores = testScores;

                return test;
            }
        }
    }
}
