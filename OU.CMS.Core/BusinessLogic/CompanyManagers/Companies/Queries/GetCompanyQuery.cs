using FluentValidation;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OU.CMS.Core.BusinessLogic.Base;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Companies.Queries
{
    public class GetCompanyQuery : BaseQuery<GetCompanyQuery>
    {
        public Guid CompanyId { get; set; }

        public GetCompanyQuery(Guid companyId)
        {
            CompanyId = companyId;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<GetCompanyQuery>
        {
            public Validator()
            {
                RuleFor(i => i.CompanyId).NotNull().NotEmpty();
            }
        }

        public async Task<GetCompanyDto> GetCompany(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                if (!userInfo.IsCandidateLogin && userInfo.CompanyId != CompanyId)
                    throw new Exception("You do not have access to perform this action!");

                var company = await (from cmp in db.Companies
                                     join usr in db.Users on cmp.CreatedBy equals usr.Id
                                     where cmp.Id == CompanyId
                                     select new GetCompanyDto
                                     {
                                         Id = cmp.Id,
                                         Name = cmp.Name,
                                         CreatedDetails = new CreatedOnDto
                                         {
                                             UserId = usr.Id,
                                             FullName = usr.FullName,
                                             ShortName = usr.ShortName,
                                             CreatedOn = cmp.CreatedOn
                                         }
                                     }).SingleOrDefaultAsync();

                if (company == null)
                    throw new Exception("Company does not exist!");

                return company;
            }
        }
    }
}
