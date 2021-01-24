using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Companies.Commands
{
    public class CreateCompanyCommand : BaseCommand<CreateCompanyCommand>
    {
        public CreateCompanyDto Dto { get; set; }

        public CreateCompanyCommand(CreateCompanyDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<CreateCompanyCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.Name).NotNull().NotEmpty();
            }
        }

        public async Task<GetCompanyDto> CreateCompany(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                if (userInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                var checkExistingCompany = db.Companies.Any(c => c.Name == Dto.Name.Trim());
                if (checkExistingCompany)
                    throw new Exception("Company with this name already exists!");

                var company = new Company
                {
                    Id = Guid.NewGuid(),
                    Name = Dto.Name.Trim(),
                    CreatedBy = userInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                var companyManagement = new CompanyManagement
                {
                    Id = Guid.NewGuid(),
                    CompanyId = company.Id,
                    UserId = userInfo.UserId,
                    IsAdmin = true,
                    CreatedBy = userInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                db.Companies.Add(company);
                db.CompanyManagements.Add(companyManagement);

                await db.SaveChangesAsync();

                return new GetCompanyDto()
                {
                    Id = company.Id,
                    Name = company.Name,
                    CreatedDetails = new CreatedOnDto
                    {
                        UserId = company.CreatedBy,
                        FullName = userInfo.FullName,
                        ShortName = userInfo.ShortName,
                        CreatedOn = company.CreatedOn
                    }
                };
            }
        }
    }
}
