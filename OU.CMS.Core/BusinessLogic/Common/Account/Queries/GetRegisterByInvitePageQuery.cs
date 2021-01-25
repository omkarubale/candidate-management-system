using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace OU.CMS.Core.BusinessLogic.Common.Account.Queries
{
    public class GetRegisterByInvitePageQuery : BaseQuery<GetRegisterByInvitePageQuery>
    {
        public string Email { get; set; }

        public Guid CompanyId { get; set; }

        public GetRegisterByInvitePageQuery(string email, Guid companyId)
        {
            Email = email;
            CompanyId = companyId;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<GetRegisterByInvitePageQuery>
        {
            public Validator()
            {
                RuleFor(i => i.Email).NotNull().NotEmpty();
                RuleFor(i => i.CompanyId).NotNull().NotEmpty();
            }
        }

        public async Task<RegisterByInvitePageDto> GetRegisterByInvitePage()
        {
            using (var db = new CMSContext())
            {
                var invite = await db.CompanyManagementInvites
                    .Include(cmi => cmi.Company)
                    .SingleOrDefaultAsync(i => i.Email == Email && i.CompanyId == CompanyId);

                if (invite == null)
                    throw new Exception("Invite for Email not found for this Company!");

                var invitedByUserName = await db.Users.Where(u => u.Id == invite.CreatedBy).Select(u => u.FullName).SingleOrDefaultAsync();

                return new RegisterByInvitePageDto()
                {
                    CompanyId = invite.CompanyId,
                    CompanyName = invite.Company.Name,
                    Email = invite.Email,
                    InvitedByUserName = invitedByUserName != null ? invitedByUserName : "Deleted User"
                };
            }
        }
    }
}
