using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Commands
{
    public class DeleteJobOpeningCommand : BaseCommand<DeleteJobOpeningCommand>
    {
        public Guid JobOpeningId { get; set; }

        public DeleteJobOpeningCommand(Guid jobOpeningId)
        {
            JobOpeningId = jobOpeningId;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<DeleteJobOpeningCommand>
        {
            public Validator()
            {
                RuleFor(i => i.JobOpeningId).NotNull().NotEmpty();
            }
        }

        public async Task DeleteJobOpening(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                if (userInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                var jobOpening = await db.JobOpenings
                    .Include(j => j.Company.CompanyManagements)
                    .SingleOrDefaultAsync(j => j.Id == JobOpeningId && j.Company.CompanyManagements.Any(cm => cm.UserId == userInfo.UserId && cm.IsAdmin));

                if (jobOpening == null)
                    throw new Exception("JobOpening with Id not found!");

                if (userInfo.CompanyId != jobOpening.CompanyId)
                    throw new Exception("You do not have access to perform this action!");

                var candidatesForJobOpening = await db.Candidates
                    .Include(c => c.CandidateTests)
                    .Include(c => c.CandidateTests.Select(ct => ct.CandidateTestScores))
                    .Where(c => c.JobOpeningId == JobOpeningId)
                    .ToListAsync();

                var candidateTestScoresForJobOpening = await db.CandidateTestScores
                    .Where(cts => cts.CandidateTest.Candidate.JobOpeningId == JobOpeningId)
                    .ToListAsync();

                db.Candidates.RemoveRange(candidatesForJobOpening);
                db.CandidateTests.RemoveRange(candidatesForJobOpening.SelectMany(c => c.CandidateTests));
                db.CandidateTestScores.RemoveRange(candidateTestScoresForJobOpening);

                db.JobOpenings.Remove(jobOpening);

                await db.SaveChangesAsync();
            }
        }
    }
}
