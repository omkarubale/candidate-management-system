using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.TestScores.Commands
{
    public class DeleteTestScoreCommand : BaseCommand<DeleteTestScoreCommand>
    {
        public Guid TestScoreId { get; set; }

        public DeleteTestScoreCommand(Guid testScoreId)
        {
            TestScoreId = testScoreId;
            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<DeleteTestScoreCommand>
        {
            public Validator()
            {
                RuleFor(i => i.TestScoreId).NotNull().NotEmpty();
            }
        }

        public async Task DeleteTestScore(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var testScoreObject = await (from cmp in db.Companies
                                        join cm in db.CompanyManagements on cmp.Id equals cm.CompanyId
                                        join t in db.Tests on cmp.Id equals t.CompanyId
                                        join ts in db.TestScores on t.Id equals ts.TestId
                                        join cts in db.CandidateTestScores on ts.Id equals cts.TestScoreId into candidateTestScoresTemp
                                        from cts in candidateTestScoresTemp.DefaultIfEmpty()
                                        where
                                        cmp.Id == userInfo.CompanyId &&
                                        cm.UserId == userInfo.UserId &&
                                        ts.Id == TestScoreId
                                        select new { ts, cts }).ToListAsync();

                if (testScoreObject == null)
                    throw new Exception("Test with Id not found!");

                var testScore = testScoreObject.Select(tso => tso.ts).Single();
                var candidateTestScores = testScoreObject.Select(tso => tso.cts).ToList();

                db.TestScores.Remove(testScore);
                db.CandidateTestScores.RemoveRange(candidateTestScores);

                await db.SaveChangesAsync();

                return;
            }
        }
    }
}
