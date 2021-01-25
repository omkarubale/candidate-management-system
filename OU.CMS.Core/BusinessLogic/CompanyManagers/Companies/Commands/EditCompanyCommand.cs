using FluentValidation;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Companies.Commands
{
    public class EditCompanyCommand
    {
        public EditCompanyDto Dto { get; set; }

        public EditCompanyCommand(EditCompanyDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<EditCompanyCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.Id).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Name).NotNull().NotEmpty();
            }
        }

        public async Task<GetCompanyDto> EditCompany(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin || userInfo.CompanyId != Dto.Id)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var checkExistingCompany = db.Companies.Any(c => c.Name == Dto.Name.Trim() && (c.Id != Dto.Id));
                if (checkExistingCompany)
                    throw new Exception("Company with this name already exists!");

                var company = await db.Companies.SingleOrDefaultAsync(c => c.Id == Dto.Id);
                if (company == null)
                    throw new Exception("Company with Id not found!");

                company.Name = Dto.Name.Trim();

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
