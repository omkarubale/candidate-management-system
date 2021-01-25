using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.User;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.Common.User.Commands
{
    public class SaveIdentityUserCommand : BaseCommand<SaveIdentityUserCommand>
    {
        public UserDto Dto { get; set; }

        public SaveIdentityUserCommand(UserDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<SaveIdentityUserCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.FirstName).NotNull().NotEmpty();
                RuleFor(i => i.Dto.LastName).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Email).NotNull().NotEmpty();
            }
        }

        public async Task<UserDto> SaveIdentityUser(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(c => c.Id == userInfo.UserId);
                if (user == null)
                    throw new Exception("User with Id not found!");

                user.FirstName = Dto.FirstName.Trim();
                user.LastName = Dto.LastName.Trim();
                user.Email = Dto.Email.Trim();

                await db.SaveChangesAsync();

                return Dto;
            }
        }
    }
}
