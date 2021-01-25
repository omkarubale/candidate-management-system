using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Core.BusinessLogic.Common.Jobs.Queries;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Candidate;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Commands
{
    public class CreateCandidateTestCommand : BaseCommand<CreateCandidateTestCommand>
    {
        public CreateCandidateTestDto Dto { get; set; }

        public CreateCandidateTestCommand(CreateCandidateTestDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<CreateCandidateTestCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.CandidateId).NotNull().NotEmpty();
                RuleFor(i => i.Dto.TestId).NotNull().NotEmpty();
            }
        }

        public async Task<CandidateTestDto> CreateCandidateTest(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var candidate = await db.Candidates.SingleOrDefaultAsync(c => c.Id == Dto.CandidateId && c.CompanyId == userInfo.CompanyId);
                if (candidate == null)
                    throw new Exception("Candidate with specified id was not found!");

                var checkExistingCandidate = db.CandidateTests.Any(c => c.CandidateId == Dto.CandidateId && c.TestId == Dto.TestId);
                if (checkExistingCandidate)
                    throw new Exception("Candidate already has this Test in his profile!");

                var candidateTest = new CandidateTest
                {
                    Id = Guid.NewGuid(),
                    CandidateId = Dto.CandidateId,
                    TestId = Dto.TestId,
                };

                db.CandidateTests.Add(candidateTest);

                var testScores = await db.TestScores.Include(ts => ts.Test).Where(c => c.TestId == Dto.TestId && c.Test.CompanyId == userInfo.CompanyId).ToListAsync();
                if (testScores == null)
                    throw new Exception("Test Score with specified id was not found!");

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

                return (await new GetCandidateTestsInternalQuery(Dto.TestId, companyId: userInfo.CompanyId, candidateId: Dto.CandidateId).GetCandidateTests()).SingleOrDefault();
            }
        }
    }
}
