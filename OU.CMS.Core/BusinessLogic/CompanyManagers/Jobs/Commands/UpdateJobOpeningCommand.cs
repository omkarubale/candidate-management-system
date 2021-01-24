using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Queries;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.JobOpening;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Commands
{
    public class UpdateJobOpeningCommand : BaseCommand<UpdateJobOpeningCommand>
    {
        public UpdateJobOpeningDto Dto { get; set; }

        public UpdateJobOpeningCommand(UpdateJobOpeningDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<UpdateJobOpeningCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.Id).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Title).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Description).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Salary).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Deadline).NotNull().NotEmpty();
            }
        }

        public async Task<GetJobOpeningDto> UpdateJobOpening(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                if (userInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                var jobOpening = await db.JobOpenings
                    .Include(j => j.Company.CompanyManagements)
                    .SingleOrDefaultAsync(j => j.Id == Dto.Id && j.Company.CompanyManagements.Any(cm => cm.UserId == userInfo.UserId && cm.IsAdmin));
                if (jobOpening == null)
                    throw new Exception("JobOpening with Id not found!");

                if (userInfo.CompanyId != jobOpening.CompanyId)
                    throw new Exception("You do not have access to perform this action!");

                var checkExistingJobOpening = db.JobOpenings.Any(c => c.Title == Dto.Title.Trim() && c.Id != Dto.Id);
                if (checkExistingJobOpening)
                    throw new Exception("JobOpening with this name already exists!");

                jobOpening.Title = Dto.Title;
                jobOpening.Description = Dto.Description;
                jobOpening.Salary = Dto.Salary;
                jobOpening.Deadline = Dto.Deadline;

                await db.SaveChangesAsync();

                return await new GetJobOpeningQuery(jobOpening.Id).GetJobOpening(userInfo);
            }
        }
    }
}
