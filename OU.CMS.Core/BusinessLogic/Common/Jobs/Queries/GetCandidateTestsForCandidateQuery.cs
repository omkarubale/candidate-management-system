using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Candidate;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.JobOpening;
using OU.CMS.Models.Models.User;
using OU.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace OU.CMS.Core.BusinessLogic.Common.Jobs.Queries
{
    public class GetCandidateTestsForCandidateQuery : BaseQuery<GetCandidateTestsForCandidateQuery>
    {
        public Guid CandidateId { get; set; }

        public GetCandidateTestsForCandidateQuery(Guid candidateId)
        {
            CandidateId = candidateId;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<GetCandidateTestsForCandidateQuery>
        {
            public Validator()
            {
                RuleFor(i => i.CandidateId).NotNull().NotEmpty();
            }
        }

        public async Task<CandidateTestsContainerDto> GetCandidateTestsForCandidate(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var candidate = await (from cnd in db.Candidates
                                       join usr in db.Users on cnd.UserId equals usr.Id
                                       join cmp in db.Companies on cnd.CompanyId equals cmp.Id
                                       join jo in db.JobOpenings on cnd.JobOpeningId equals jo.Id
                                       join lc in db.Users on cnd.CreatedBy equals lc.Id
                                       where
                                       cnd.Id == CandidateId &&
                                       cnd.CompanyId == userInfo.CompanyId
                                       select new GetCandidateDto
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
                                       }).SingleOrDefaultAsync();

                if (candidate == null)
                    throw new Exception("Candidate not found!");

                var candidateTests = (await (from cdt in db.CandidateTests
                                             join cd in db.Candidates on cdt.CandidateId equals cd.Id
                                             join tst in db.Tests on cdt.TestId equals tst.Id
                                             join cdts in db.CandidateTestScores on cdt.Id equals cdts.CandidateTestId
                                             join tsts in db.TestScores on cdts.TestScoreId equals tsts.Id
                                             where
                                             cdt.CandidateId == CandidateId &&
                                             cd.CompanyId == userInfo.CompanyId
                                             select new
                                             {
                                                 CandidateId = cdt.CandidateId,
                                                 TestTitle = tst.Title,

                                                 TestScoreId = tsts.Id,
                                                 TestScoreTitle = tsts.Title,
                                                 TestScoreIsMandatory = tsts.IsMandatory,
                                                 TestScoreMaximumScore = tsts.MaximumScore,
                                                 TestScoreCutoffScore = tsts.CutoffScore,

                                                 CandidateTestScoreId = cdts.Id,
                                                 CandidateTestScoreValue = cdts.Value,
                                                 CandidateTestScoreComment = cdts.Comment
                                             }).ToListAsync())
                                            .GroupBy(t => new { t.CandidateId, t.TestTitle })
                                            .Select(t => new CandidateTestDto
                                            {
                                                Title = t.Key.TestTitle,

                                                CandidateTestScores = t.Select(ts => new CandidateTestScoreDto
                                                {
                                                    CandidateTestScoreId = ts.CandidateTestScoreId,
                                                    TestScoreId = ts.TestScoreId,
                                                    Title = ts.TestScoreTitle,
                                                    IsMandatory = ts.TestScoreIsMandatory,

                                                    Value = ts.CandidateTestScoreValue,
                                                    MaximumScore = ts.TestScoreMaximumScore,
                                                    CutoffScore = ts.TestScoreCutoffScore,
                                                    Comment = ts.CandidateTestScoreComment
                                                }).OrderBy(ts => ts.Title).ToList()
                                            }).ToList();

                var sortedTotalScores = candidateTests.Select(cdsts => (double?)cdsts.CandidateTestScores.Select(cdts => cdts.Value)?.Sum() ?? 0d).ToList();
                sortedTotalScores.Sort();

                foreach (var candidateTest in candidateTests)
                {
                    candidateTest.TotalScore = candidateTest.CandidateTestScores.Select(ts => ts.Value)?.Sum() ?? 0;
                    candidateTest.Percentile = (decimal)sortedTotalScores.GetPercentile((double)candidateTest.TotalScore);
                }

                return new CandidateTestsContainerDto
                {
                    Candidate = candidate,
                    CandidateTests = candidateTests
                };
            }
        }
    }
}
