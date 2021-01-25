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
    public class RevokeCompanyManagementInviteCommand : BaseCommand<RevokeCompanyManagementInviteCommand>
    {
        public RevokeCompanyManagementInviteDto Dto { get; set; }

        public RevokeCompanyManagementInviteCommand(RevokeCompanyManagementInviteDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<RevokeCompanyManagementInviteCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.CompanyId).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Email).NotNull().NotEmpty();
            }
        }

        public async Task RevokeCompanyManagementInvite(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                var companyManagerAccess = await db.CompanyManagements.AnyAsync(c => c.CompanyId == Dto.CompanyId && c.UserId == userInfo.UserId && c.IsAdmin);
                if (!companyManagerAccess)
                    throw new Exception("You do not have access to perform this action!");

                var companyManagementInvite = await db.CompanyManagementInvites.SingleAsync(c => c.Email == Dto.Email.Trim() && c.CompanyId == Dto.CompanyId);
                if (companyManagementInvite == null)
                    throw new Exception("Company Management Invite for this Email doesn't exist!");

                var checkExistingManagement = await (from cm in db.CompanyManagements
                                                     join usr in db.Users on cm.UserId equals usr.Id
                                                     where
                                                     cm.CompanyId == Dto.CompanyId &&
                                                     usr.Email == Dto.Email.Trim()
                                                     select cm).AnyAsync();
                if (checkExistingManagement)
                    throw new Exception("This User is already a part of this company!");

                db.CompanyManagementInvites.Remove(companyManagementInvite);

                await db.SaveChangesAsync();
            }
        }
    }
}
