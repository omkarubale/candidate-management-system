using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OU.CMS.Core.BusinessLogic.Base;

namespace OU.CMS.Core.BusinessLogic.Candidates.Companies.Queries
{
    public class GetAllCompaniesQuery : BaseQuery<GetAllCompaniesQuery>
    {
        public async Task<List<GetCompanyDto>> GetAllCompanies(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                if (!userInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                var companies = await (from cmp in db.Companies
                                       join usr in db.Users on cmp.CreatedBy equals usr.Id
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
                                       }).ToListAsync();

                return companies;
            }
        }
    }
}
