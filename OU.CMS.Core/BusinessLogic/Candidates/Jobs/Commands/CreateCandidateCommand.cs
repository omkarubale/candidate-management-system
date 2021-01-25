using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Core.BusinessLogic.Common.Jobs.Queries;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Candidate;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.Candidates.Jobs.Commands
{
    public class CreateCandidateCommand : BaseCommand<CreateCandidateCommand>
    {
        public CreateCandidateDto Dto { get; set; }

        public CreateCandidateCommand(CreateCandidateDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<CreateCandidateCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.CompanyId).NotNull().NotEmpty();
                RuleFor(i => i.Dto.UserId).NotNull().NotEmpty();
                RuleFor(i => i.Dto.JobOpeningId).NotNull().NotEmpty();
            }
        }

        public async Task<GetCandidateDto> CreateCandidate(UserInfo userInfo)
        {
            if (!userInfo.IsCandidateLogin || userInfo.UserId != Dto.UserId)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var checkExistingCandidate = db.Candidates.Any(c => c.UserId == Dto.UserId && c.JobOpeningId == Dto.JobOpeningId);
                if (checkExistingCandidate)
                    throw new Exception("Candidate has already applied for this Job Position!");

                var candidate = new Candidate
                {
                    Id = Guid.NewGuid(),
                    UserId = Dto.UserId,
                    CompanyId = Dto.CompanyId,
                    JobOpeningId = Dto.JobOpeningId,
                    CreatedBy = userInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                db.Candidates.Add(candidate);

                await db.SaveChangesAsync();

                return (await new GetCandidatesInternalQuery(candidateId: candidate.Id, userId: userInfo.UserId).GetAllCandidates()).SingleOrDefault();
            }
        }
    }
}
