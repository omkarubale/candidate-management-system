using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Queries;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.JobOpening;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Commands
{
    public class CreateJobOpeningCommand : BaseCommand<CreateJobOpeningCommand>
    {
        public CreateJobOpeningDto Dto { get; set; }

        public CreateJobOpeningCommand(CreateJobOpeningDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<CreateJobOpeningCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.CompanyId).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Title).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Description).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Salary).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Deadline).NotNull().NotEmpty();
            }
        }

        public async Task<GetJobOpeningDto> CreateJobOpening(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin || userInfo.CompanyId != Dto.CompanyId)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var checkExistingJobOpening = db.JobOpenings.Any(c => c.Title == Dto.Title.Trim());
                if (checkExistingJobOpening)
                    throw new Exception("JobOpening with this title already exists!");

                var comanyManagementAccess = db.CompanyManagements.Any(cm => cm.UserId == userInfo.UserId && cm.CompanyId == Dto.CompanyId && cm.IsAdmin);
                if (comanyManagementAccess)
                    throw new Exception("You do not have access to perform this action!");

                var jobOpening = new JobOpening
                {
                    Id = Guid.NewGuid(),
                    Title = Dto.Title.Trim(),
                    Description = Dto.Description.Trim(),
                    Salary = Dto.Salary,
                    Deadline = Dto.Deadline,
                    CompanyId = Dto.CompanyId,
                    CreatedBy = userInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                db.JobOpenings.Add(jobOpening);

                await db.SaveChangesAsync();

                return await new GetJobOpeningQuery(jobOpening.Id).GetJobOpening(userInfo);
            }
        }
    }
}
