using FluentValidation;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Company;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OU.CMS.Core.BusinessLogic.Base;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.CompanyManagements.Commands
{
    public class CreateCompanyManagementInviteCommand : BaseCommand<CreateCompanyManagementInviteCommand>
    {
        public CreateCompanyManagementInviteDto Dto { get; set; }

        public CreateCompanyManagementInviteCommand(CreateCompanyManagementInviteDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<CreateCompanyManagementInviteCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.CompanyId).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Email).NotNull().NotEmpty();
            }
        }

        public async Task CreateCompanyManagementInvite(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                var companyManagerAccess = await db.CompanyManagements.AnyAsync(c => c.CompanyId == Dto.CompanyId && c.UserId == userInfo.UserId && c.IsAdmin);
                if (!companyManagerAccess)
                    throw new Exception("You do not have access to perform this action!");

                var checkExistingInvite = await db.CompanyManagementInvites.AnyAsync(c => c.Email == Dto.Email.Trim() && c.CompanyId == Dto.CompanyId);
                if (checkExistingInvite)
                    throw new Exception("Company Management Invite for this Email already exists!");

                var checkExistingManagement = await (from cm in db.CompanyManagements
                                                     join usr in db.Users on cm.UserId equals usr.Id
                                                     where
                                                     cm.CompanyId == Dto.CompanyId &&
                                                     usr.Email == Dto.Email.Trim()
                                                     select cm).AnyAsync();
                if (checkExistingManagement)
                    throw new Exception("This User is already a part of this company!");

                var companyManagementInvite = new CompanyManagementInvite
                {
                    Id = Guid.NewGuid(),
                    CompanyId = Dto.CompanyId,
                    Email = Dto.Email,
                    IsInviteForAdmin = Dto.IsInviteForAdmin,
                    CreatedBy = userInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                db.CompanyManagementInvites.Add(companyManagementInvite);

                await db.SaveChangesAsync();
            }
        }
    }
}
