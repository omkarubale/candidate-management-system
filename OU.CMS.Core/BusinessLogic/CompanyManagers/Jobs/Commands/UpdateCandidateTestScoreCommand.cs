using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Candidate;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Commands
{
    public class UpdateCandidateTestScoreCommand : BaseCommand<UpdateCandidateTestScoreCommand>
    {
        public UpdateCandidateTestScoreDto Dto { get; set; }

        public UpdateCandidateTestScoreCommand(UpdateCandidateTestScoreDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<UpdateCandidateTestScoreCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.CandidateTestScoreId).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Value).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Comment).NotNull().NotEmpty();
            }
        }

        public async Task UpdateCandidateTestScore(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var candidateTestScore = await db.CandidateTestScores
                    .Include(cts => cts.TestScore)
                    .Include(cts => cts.CandidateTest)
                    .Include(cts => cts.CandidateTest.Candidate)
                    .SingleOrDefaultAsync(cts => cts.Id == Dto.CandidateTestScoreId && cts.CandidateTest.Candidate.CompanyId == userInfo.CompanyId);

                if (candidateTestScore == null)
                    throw new Exception("Candidate Test Score doesn't exist!");

                if (candidateTestScore.TestScore.MinimumScore > Dto.Value)
                    throw new Exception("Test Score has to be more than Minimum Value!");

                if (candidateTestScore.TestScore.MaximumScore < Dto.Value)
                    throw new Exception("Test Score has to be less than Maximum Value!");

                candidateTestScore.Value = Dto.Value;
                candidateTestScore.Comment = Dto.Comment;

                await db.SaveChangesAsync();
            }
        }
    }
}
