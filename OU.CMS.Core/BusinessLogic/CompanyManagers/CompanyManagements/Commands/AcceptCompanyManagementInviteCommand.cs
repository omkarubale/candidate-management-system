using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Company;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.CompanyManagements.Commands
{
    public class AcceptCompanyManagementInviteCommand : BaseCommand<AcceptCompanyManagementInviteCommand>
    {
        public AcceptCompanyManagementInviteDto Dto { get; set; }

        public AcceptCompanyManagementInviteCommand(AcceptCompanyManagementInviteDto dto)
        {
            Dto = dto;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<AcceptCompanyManagementInviteCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.CompanyId).NotNull().NotEmpty();
            }
        }

        public async Task AcceptCompanyManagementInvite(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                var companyManagementInvite = await db.CompanyManagementInvites.SingleOrDefaultAsync(c => c.Email == userInfo.Email && c.CompanyId == Dto.CompanyId);
                if (companyManagementInvite == null)
                    throw new Exception("Company Management Invite for this User doesn't exist!");

                db.CompanyManagementInvites.Remove(companyManagementInvite);

                var companyManagement = new CompanyManagement
                {
                    Id = Guid.NewGuid(),
                    CompanyId = Dto.CompanyId,
                    UserId = userInfo.UserId,
                    IsAdmin = companyManagementInvite.IsInviteForAdmin,
                    CreatedBy = userInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                db.CompanyManagements.Add(companyManagement);

                await db.SaveChangesAsync();
            }
        }
    }
}
