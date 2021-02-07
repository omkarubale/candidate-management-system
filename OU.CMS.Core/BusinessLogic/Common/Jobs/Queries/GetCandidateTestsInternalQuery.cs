using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
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
    public class GetCandidateTestsInternalQuery : BaseQuery<GetCandidateTestsInternalQuery>
    {
        public Guid TestId { get; set; }

        public Guid? UserId { get; set; }

        public Guid? CompanyId { get; set; }

        public Guid? CandidateId { get; set; }

        public GetCandidateTestsInternalQuery(Guid testId, Guid? userId = null, Guid? companyId = null, Guid? candidateId = null)
        {
            TestId = testId;
            UserId = userId;
            CompanyId = companyId;
            CandidateId = candidateId;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<GetCandidateTestsInternalQuery>
        {
            public Validator()
            {
                RuleFor(i => i.TestId).NotNull().NotEmpty();
            }
        }

        public async Task<List<CandidateTestDto>> GetCandidateTests()
        {
            using (var db = new CMSContext())
            {
                var isCandidateFilter = CandidateId != null && CandidateId != Guid.Empty;
                var isCompanyFilter = CompanyId != null && CompanyId != Guid.Empty;
                var isUserFilter = UserId != null && UserId != Guid.Empty;

                var candidates = await (from cdt in db.CandidateTests
                                        join cnd in db.Candidates on cdt.CandidateId equals cnd.Id
                                        join usr in db.Users on cnd.UserId equals usr.Id
                                        join cmp in db.Companies on cnd.CompanyId equals cmp.Id
                                        join jo in db.JobOpenings on cnd.JobOpeningId equals jo.Id
                                        join lc in db.Users on cnd.CreatedBy equals lc.Id
                                        where
                                        cdt.TestId == TestId &&
                                        (!isCandidateFilter || cnd.Id == CandidateId) &&
                                        (!isCompanyFilter || cnd.CompanyId == CompanyId) &&
                                        (!isUserFilter || cnd.UserId == UserId)
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
                                                   join cd in db.Candidates on cdt.CandidateId equals cd.Id
                                                   join tst in db.Tests on cdt.TestId equals tst.Id
                                                   join cdts in db.CandidateTestScores on cdt.Id equals cdts.CandidateTestId
                                                   join tsts in db.TestScores on cdts.TestScoreId equals tsts.Id
                                                   where
                                                   cdt.TestId == TestId &&
                                                   (!isCandidateFilter || cdt.CandidateId == CandidateId) &&
                                                   (!isCompanyFilter || cd.CompanyId == CompanyId) &&
                                                   (!isUserFilter || cd.UserId == UserId)
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
                                                    MaximumScore = ts.TestScoreMaximumScore,
                                                    CutoffScore = ts.TestScoreCutoffScore,
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
        }
    }
}
