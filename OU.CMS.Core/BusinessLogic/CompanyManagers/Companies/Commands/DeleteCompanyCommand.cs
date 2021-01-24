using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Companies.Commands
{
    public class DeleteCompanyCommand : BaseCommand<DeleteCompanyCommand>
    {
        public Guid CompanyId { get; set; }

        public DeleteCompanyCommand(Guid companyId)
        {
            CompanyId = companyId;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<DeleteCompanyCommand>
        {
            public Validator()
            {
                RuleFor(i => i.CompanyId).NotNull().NotEmpty();
            }
        }

        public async Task DeleteCompany(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                if (userInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                var company = await db.Companies.Include(c => c.JobOpenings).Include(c => c.CompanyManagements).Include(c => c.CompanyManagementInvites).SingleOrDefaultAsync(c => c.Id == CompanyId);
                if (company == null)
                    throw new Exception("Company with Id not found!");

                var companyManagerAccess = company.CompanyManagements.SingleOrDefault(c => c.CompanyId == CompanyId && c.UserId == userInfo.UserId && c.IsAdmin);
                if (companyManagerAccess == null)
                    throw new Exception("You do not have access to perform this action!");

                var companyCandidates = await db.Candidates.AnyAsync(c => c.CompanyId == CompanyId);
                if (companyCandidates)
                    throw new Exception("Company cannot be deleted as candidates have applied for this company's job Openings!");

                db.CompanyManagementInvites.RemoveRange(company.CompanyManagementInvites);
                db.CompanyManagements.RemoveRange(company.CompanyManagements);
                db.JobOpenings.RemoveRange(company.JobOpenings);

                db.Companies.Remove(company);

                await db.SaveChangesAsync();
            }
        }
    }
}
