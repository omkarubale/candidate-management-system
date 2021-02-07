using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Common.Lookup;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.Test;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Tests.Queries
{
    public class GetTestsAsCompanyManagerForLookupQuery : BaseQuery<GetTestsAsCompanyManagerForLookupQuery>
    {
        public async Task<List<LookupDto<Guid>>> GetTestsAsCompanyManagerForLookup(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var tests = await (from tst in db.Tests
                                   join cmp in db.Companies on tst.CompanyId equals cmp.Id
                                   join cm in db.CompanyManagements on cmp.Id equals cm.CompanyId
                                   where
                                   tst.CompanyId == (Guid)userInfo.CompanyId &&
                                   cm.UserId == userInfo.UserId
                                   select new LookupDto<Guid>
                                   {
                                       Id = tst.Id,
                                       Name = tst.Title,
                                       Description = tst.Description,
                                   }).OrderBy(t => t.Name).ToListAsync();

                return tests;
            }
        }
    }
}
