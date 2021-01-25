using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Test;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.TestScores.Commands
{
    public class UpdateTestScoreCommand : BaseCommand<UpdateTestScoreCommand>
    {
        public UpdateTestScoreDto Dto { get; set; }

        public UpdateTestScoreCommand(UpdateTestScoreDto dto)
        {
            Dto = dto;
            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<UpdateTestScoreCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.Id).NotNull().NotEmpty();
                RuleFor(i => i.Dto.TestId).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Title).NotNull().NotEmpty();
                RuleFor(i => i.Dto.MinimumScore).NotNull();
                RuleFor(i => i.Dto.MaximumScore).NotNull().NotEmpty();
                RuleFor(i => i.Dto).Must(dto => dto.CutoffScore == null || (dto.CutoffScore > dto.MinimumScore && dto.CutoffScore < dto.MaximumScore)).WithMessage("The Cutoff score must be between Minimum score and Maximum score!");
            }
        }

        public async Task<TestScoreDto> UpdateTestScore(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var companyManagementAccess = await db.CompanyManagements.Include(cm => cm.Company.Tests).AnyAsync(cm => cm.CompanyId == userInfo.CompanyId && cm.UserId == userInfo.UserId && cm.Company.Tests.Any(t => t.Id == Dto.TestId));
                if (!companyManagementAccess)
                    throw new Exception("You do not have access to perform this action!");

                var testScore = await db.TestScores.Include(ts => ts.Test).SingleOrDefaultAsync(ts => ts.Id == Dto.Id && ts.Test.CompanyId == userInfo.CompanyId);
                if (testScore == null)
                    throw new Exception("Test with Id not found!");

                var checkExistingTestScore = db.TestScores.Include(ts => ts.Test).Any(ts => ts.Title == Dto.Title.Trim() && ts.TestId == Dto.TestId && ts.Test.CompanyId == userInfo.CompanyId && ts.Id != Dto.Id);
                if (checkExistingTestScore)
                    throw new Exception("Test with this title already exists!");

                testScore.Title = Dto.Title;
                testScore.IsMandatory = Dto.IsMandatory;
                testScore.MinimumScore = Dto.MinimumScore;
                testScore.MaximumScore = Dto.MaximumScore;
                testScore.CutoffScore = Dto.CutoffScore;

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
