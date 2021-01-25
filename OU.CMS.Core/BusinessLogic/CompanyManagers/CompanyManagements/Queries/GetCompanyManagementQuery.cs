using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.User;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.CompanyManagements.Queries
{
    public class GetCompanyManagementQuery : BaseQuery<GetCompanyManagementQuery>
    {
        public Guid CompanyId { get; set; }

        public GetCompanyManagementQuery(Guid companyId)
        {
            CompanyId = companyId;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<GetCompanyManagementQuery>
        {
            public Validator()
            {
                RuleFor(i => i.CompanyId).NotNull().NotEmpty();
            }
        }

        public async Task<GetCompanyManagementDto> GetCompanyManagement(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin || userInfo.CompanyId != CompanyId)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var companyManagers = await db.CompanyManagements.Where(cm => cm.CompanyId == CompanyId).Include(cm => cm.User).Select(cm => new CompanyManagerDto()
                {
                    User = new UserSimpleDto()
                    {
                        UserId = cm.User.Id,
                        Email = cm.User.Email,
                        FullName = cm.User.FullName,
                        ShortName = cm.User.ShortName
                    },
                    IsAdmin = cm.IsAdmin,
                    HasAcceptedInvite = true,
                    InviteeEmail = cm.User.Email
                }).OrderByDescending(cm => cm.IsAdmin).ThenBy(cm => cm.User.FullName).ToListAsync();

                var companyManagerInvites = await db.CompanyManagementInvites.Where(cmi => cmi.CompanyId == CompanyId).Select(cmi => new CompanyManagerDto()
                {
                    User = new UserSimpleDto(),
                    IsAdmin = cmi.IsInviteForAdmin,
                    HasAcceptedInvite = false,
                    InviteeEmail = cmi.Email
                }).OrderByDescending(cm => cm.IsAdmin).ThenBy(cm => cm.InviteeEmail).ToListAsync();

                var resultList = (companyManagers).Union(companyManagerInvites).ToList();

                var result = new GetCompanyManagementDto
                {
                    CompanyManagers = resultList
                };

                return result;
            }
        }
    }
}
