using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Lookups;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Account;
using OU.Utility.Services;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.Common.Account.Commands
{
    public class LoginCommand : BaseCommand<LoginCommand>
    {
        public SignInDto Dto { get; set; }

        public LoginCommand(SignInDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<LoginCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.Email).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Password).NotNull().NotEmpty();
                RuleFor(i => i.Dto.UserType).NotNull().NotEmpty();
            }
        }

        public async Task<UserInfo> Login()
        {
            Dto.Email = Dto.Email.Trim().ToLower();

            using (var db = new CMSContext())
            {
                var user = await db.Users.SingleOrDefaultAsync(u => u.Email == Dto.Email && u.UserType == (UserType)Dto.UserType);

                // User doesn't exist
                if (user == null)
                    throw new Exception("User does not exist or Password is not correct!");

                // Password Incorrect
                if (!PasswordService.VerifyPasswordHash(Dto.Password, user.PasswordHash, user.PasswordSalt))
                    throw new Exception("User does not exist or Password is not correct!");

                var companyId = user.UserType == UserType.Management ? user.DefaultCompanyId : null;

                return new UserInfo()
                {
                    //Token is generated in controller using these details
                    UserId = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ShortName = user.ShortName,
                    FullName = user.FullName,
                    UserType = user.UserType,
                    IsCandidateLogin = user.UserType == UserType.Candidate,
                    CompanyId = companyId
                };
            }
        }
    }
}
