using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Tests.Commands
{
    public class DeleteTestCommand : BaseCommand<DeleteTestCommand>
    {
        public Guid TestId { get; set; }

        public DeleteTestCommand(Guid testId)
        {
            TestId = testId;
            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<DeleteTestCommand>
        {
            public Validator()
            {
                RuleFor(i => i.TestId).NotNull().NotEmpty();
            }
        }

        public async Task DeleteTest(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var companyManagementAccess = await db.CompanyManagements.AnyAsync(cm => cm.CompanyId == userInfo.CompanyId && cm.UserId == userInfo.UserId && cm.IsAdmin);
                if (!companyManagementAccess)
                    throw new Exception("You do not have access to perform this action!");

                var test = await db.Tests
                    .Include(t => t.TestScores)
                    .Include(t => t.CandidateTests)
                    .SingleOrDefaultAsync(c => c.Id == TestId && c.CompanyId == userInfo.CompanyId);
                if (test == null)
                    throw new Exception("Test with Id not found!");

                var candidateTestScores = await db.CandidateTestScores
                    .Where(c => c.CandidateTest.TestId == TestId && c.CandidateTest.Candidate.CompanyId == userInfo.CompanyId)
                    .ToListAsync();

                if (candidateTestScores.Any())
                    db.CandidateTestScores.RemoveRange(candidateTestScores);
                db.CandidateTests.RemoveRange(test.CandidateTests);
                db.TestScores.RemoveRange(test.TestScores);
                db.Tests.Remove(test);

                await db.SaveChangesAsync();
            }
        }
    }
}
