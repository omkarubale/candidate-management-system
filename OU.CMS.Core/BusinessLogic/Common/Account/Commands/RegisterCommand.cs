using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Lookups;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Account;
using OU.Utility.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.Common.Account.Commands
{
    public class RegisterCommand : BaseCommand<RegisterCommand>
    {
        public SignUpDto Dto { get; set; }

        public RegisterCommand(SignUpDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<RegisterCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.Email).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Password).NotNull().NotEmpty();
                RuleFor(i => i.Dto.ConfirmPassword).NotNull().NotEmpty();
                RuleFor(i => i.Dto.FirstName).NotNull().NotEmpty();
                RuleFor(i => i.Dto.LastName).NotNull().NotEmpty();
                RuleFor(i => i.Dto.CompanyName).NotNull().NotEmpty().When(i => !i.Dto.IsCandidateLogin);
                RuleFor(i => i.Dto).Must(i => i.ConfirmPassword == i.Password).WithMessage("Password confirmation does not match!");
            }
        }

        public async Task Register()
        {
            using (var db = new CMSContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.Email == Dto.Email);

                // User already exists
                if (user != null)
                    throw new Exception("User with this email already exists!");

                byte[] passwordHash, passwordSalt;
                PasswordService.CreatePasswordHash(Dto.Password, out passwordHash, out passwordSalt);

                user = new Domain.Entities.User()
                {
                    Id = Guid.NewGuid(),
                    Email = Dto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    UserType = Dto.IsCandidateLogin ? UserType.Candidate : UserType.Management,

                    FullName = Dto.FirstName + " " + Dto.LastName,
                    FirstName = Dto.FirstName,
                    LastName = Dto.LastName,
                    ShortName = (Dto.FirstName.ToCharArray().First().ToString() + Dto.LastName.ToCharArray().First().ToString()).ToUpper(),
                    // TODO: Add Default company when getting invite for company Manager
                };

                if (!Dto.IsCandidateLogin)
                {
                    var company = new Domain.Entities.Company()
                    {
                        Id = Guid.NewGuid(),
                        Name = Dto.CompanyName,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = user.Id
                    };

                    var companyManagement = new Domain.Entities.CompanyManagement()
                    {
                        Id = Guid.NewGuid(),
                        CompanyId = company.Id,
                        UserId = user.Id,
                        IsAdmin = true,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = user.Id
                    };

                    user.DefaultCompanyId = company.Id;

                    db.Users.Add(user);
                    db.Companies.Add(company);
                    db.CompanyManagements.Add(companyManagement);
                }
                else
                {
                    db.Users.Add(user);
                }

                await db.SaveChangesAsync();
            }
        }
    }
}
