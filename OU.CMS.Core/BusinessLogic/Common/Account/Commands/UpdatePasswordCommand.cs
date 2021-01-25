using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Account;
using OU.Utility.Services;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.Common.Account.Commands
{
    public class UpdatePasswordCommand : BaseCommand<UpdatePasswordCommand>
    {
        public UpdatePasswordDto Dto { get; set; }

        public UpdatePasswordCommand(UpdatePasswordDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<UpdatePasswordCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.Password).NotNull().NotEmpty();
                RuleFor(i => i.Dto.ConfirmPassword).NotNull().NotEmpty();
                RuleFor(i => i.Dto).Must(i => i.ConfirmPassword == i.Password).WithMessage("Password confirmation does not match!");
            }
        }

        public async Task UpdatePassword(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.Id == userInfo.UserId);

                // User doesn't exist
                if (user == null)
                    throw new Exception("User with this email already exists!");

                byte[] passwordHash, passwordSalt;
                PasswordService.CreatePasswordHash(Dto.Password, out passwordHash, out passwordSalt);

                user.PasswordSalt = passwordSalt;
                user.PasswordHash = passwordHash;

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
