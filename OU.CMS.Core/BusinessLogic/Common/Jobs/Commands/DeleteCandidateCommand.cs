using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.Common.Jobs.Commands
{
    public class DeleteCandidateCommand : BaseCommand<DeleteCandidateCommand>
    {
        public Guid CandidateId { get; set; }

        public DeleteCandidateCommand(Guid candidateId)
        {
            CandidateId = candidateId; 
            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<DeleteCandidateCommand>
        {
            public Validator()
            {
                RuleFor(i => i.CandidateId).NotNull().NotEmpty();
            }
        }


        public async Task DeleteCandidate(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                var candidate = await db.Candidates.SingleOrDefaultAsync(c => c.Id == CandidateId);

                if (candidate == null)
                    throw new Exception("Candidate with Id not found!");

                if ((!userInfo.IsCandidateLogin && userInfo.CompanyId != candidate.CompanyId) || (userInfo.IsCandidateLogin && userInfo.UserId != candidate.UserId))
                    throw new Exception("You do not have access to perform this action!");

                var candidateTests = await db.CandidateTests
                    .Include(ct => ct.CandidateTestScores)
                    .Where(ct => ct.CandidateId == CandidateId).ToListAsync();

                db.CandidateTestScores.RemoveRange(candidateTests.SelectMany(ct => ct.CandidateTestScores));
                db.CandidateTests.RemoveRange(candidateTests);
                db.Candidates.Remove(candidate);

                await db.SaveChangesAsync();
            }
        }
    }
}
