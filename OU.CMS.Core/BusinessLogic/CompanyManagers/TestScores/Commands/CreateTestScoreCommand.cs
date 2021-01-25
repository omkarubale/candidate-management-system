using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Test;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.TestScores.Commands
{
    public class CreateTestScoreCommand : BaseCommand<CreateTestScoreCommand>
    {
        public CreateTestScoreDto Dto { get; set; }

        public CreateTestScoreCommand(CreateTestScoreDto dto)
        {
            Dto = dto;
            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<CreateTestScoreCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.TestId).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Title).NotNull().NotEmpty();
                RuleFor(i => i.Dto.MinimumScore).NotNull();
                RuleFor(i => i.Dto.MaximumScore).NotNull().NotEmpty();
                RuleFor(i => i.Dto).Must(dto => dto.CutoffScore == null || (dto.CutoffScore > dto.MinimumScore && dto.CutoffScore < dto.MaximumScore)).WithMessage("The Cutoff score must be between Minimum score and Maximum score!");
            }
        }

        public async Task<TestScoreDto> CreateTestScore(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var companyManagementAccess = await db.CompanyManagements.Include(cm => cm.Company.Tests).AnyAsync(cm => cm.CompanyId == userInfo.CompanyId && cm.UserId == userInfo.UserId && cm.Company.Tests.Any(t => t.Id == Dto.TestId));
                if (!companyManagementAccess)
                    throw new Exception("You do not have access to perform this action!");

                var checkExistingTest = db.TestScores.Include(ts => ts.Test).Any(ts => ts.Title == Dto.Title.Trim() && ts.TestId == Dto.TestId && ts.Test.CompanyId == userInfo.CompanyId);
                if (checkExistingTest)
                    throw new Exception("Test Score with this title already exists in this Test!");

                var testScore = new TestScore
                {
                    Id = Guid.NewGuid(),
                    TestId = Dto.TestId,
                    Title = Dto.Title.Trim(),
                    IsMandatory = Dto.IsMandatory,
                    MinimumScore = Dto.MinimumScore,
                    MaximumScore = Dto.MaximumScore,
                    CutoffScore = Dto.CutoffScore
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
    }
}