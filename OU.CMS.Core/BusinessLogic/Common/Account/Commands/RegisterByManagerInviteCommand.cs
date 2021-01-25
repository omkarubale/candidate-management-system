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
    public class RegisterByManagerInviteCommand : BaseCommand<RegisterByManagerInviteCommand>
    {
        public SignUpByManagerInviteDto Dto { get; set; }

        public RegisterByManagerInviteCommand(SignUpByManagerInviteDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<RegisterByManagerInviteCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.Email).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Password).NotNull().NotEmpty();
                RuleFor(i => i.Dto.ConfirmPassword).NotNull().NotEmpty();
                RuleFor(i => i.Dto.FirstName).NotNull().NotEmpty();
                RuleFor(i => i.Dto.LastName).NotNull().NotEmpty();
                RuleFor(i => i.Dto.CompanyId).NotNull().NotEmpty();
                RuleFor(i => i.Dto).Must(i => i.ConfirmPassword == i.Password).WithMessage("Password confirmation does not match!");
            }
        }

        public async Task RegisterByManagerInvite()
        {
            using (var db = new CMSContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.Email == Dto.Email);

                // User already exists
                if (user != null)
                    throw new Exception("User with this email already exists!");

                var invite = await db.CompanyManagementInvites.SingleOrDefaultAsync(i => i.CompanyId == Dto.CompanyId && i.Email == Dto.Email.Trim());

                // Invite doesn't exist
                if (invite != null)
                    throw new Exception("Invite for this company does not exist for this email!");

                byte[] passwordHash, passwordSalt;
                PasswordService.CreatePasswordHash(Dto.Password, out passwordHash, out passwordSalt);

                user = new Domain.Entities.User()
                {
                    Id = Guid.NewGuid(),
                    Email = Dto.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    UserType = UserType.Management,

                    FullName = Dto.FirstName + " " + Dto.LastName,
                    FirstName = Dto.FirstName,
                    LastName = Dto.LastName,
                    ShortName = (Dto.FirstName.ToCharArray().First().ToString() + Dto.LastName.ToCharArray().First().ToString()).ToUpper(),
                    DefaultCompanyId = invite.CompanyId,
                };

                var companyManagement = new Domain.Entities.CompanyManagement()
                {
                    Id = Guid.NewGuid(),
                    CompanyId = invite.CompanyId,
                    UserId = user.Id,
                    IsAdmin = invite.IsInviteForAdmin,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = user.Id
                };

                db.Users.Add(user);
                db.CompanyManagements.Add(companyManagement);

                await db.SaveChangesAsync();
            }
        }
    }
}
