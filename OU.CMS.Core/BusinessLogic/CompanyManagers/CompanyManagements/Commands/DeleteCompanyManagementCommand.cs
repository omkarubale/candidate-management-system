using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Company;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.CompanyManagements.Commands
{
    public class DeleteCompanyManagementCommand : BaseCommand<DeleteCompanyManagementCommand>
    {
        public DeleteCompanyManagementDto Dto { get; set; }

        public DeleteCompanyManagementCommand(DeleteCompanyManagementDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<DeleteCompanyManagementCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.CompanyId).NotNull().NotEmpty();
                RuleFor(i => i.Dto.UserId).NotNull().NotEmpty();
            }
        }

        public async Task DeleteCompanyManagement(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                if (userInfo.IsCandidateLogin || userInfo.CompanyId != Dto.CompanyId)
                    throw new Exception("You do not have access to perform this action!");

                var company = await db.Companies.Include(c => c.JobOpenings).Include(c => c.CompanyManagements).SingleOrDefaultAsync(c => c.Id == Dto.CompanyId);
                if (company == null)
                    throw new Exception("Company with Id not found!");

                var companyManagerAccess = company.CompanyManagements.Any(c => c.CompanyId == Dto.CompanyId && c.UserId == userInfo.UserId && c.IsAdmin);
                if (!companyManagerAccess)
                    throw new Exception("You do not have access to perform this action!");

                var companyManagement = company.CompanyManagements.SingleOrDefault(c => c.CompanyId == Dto.CompanyId && c.UserId == Dto.UserId);
                if (companyManagement == null)
                    throw new Exception("This User is not a part of this Company!");

                db.CompanyManagements.Remove(companyManagement);

                await db.SaveChangesAsync();
            }
        }
    }
}
