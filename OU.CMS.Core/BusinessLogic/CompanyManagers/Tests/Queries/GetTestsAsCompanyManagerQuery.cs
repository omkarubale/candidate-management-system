using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.Test;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Tests.Queries
{
    public class GetTestsAsCompanyManagerQuery : BaseQuery<GetTestsAsCompanyManagerQuery>
    {
        public async Task<List<GetTestDto>> GetTestsAsCompanyManager(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var testsQuery = (from tst in db.Tests
                                  join cmp in db.Companies on tst.CompanyId equals cmp.Id
                                  join cm in db.CompanyManagements on cmp.Id equals cm.CompanyId
                                  join usr in db.Users on tst.CreatedBy equals usr.Id
                                  join cdt in db.CandidateTests on tst.Id equals cdt.TestId into candidateTestTemp
                                  from cdt in candidateTestTemp.DefaultIfEmpty()
                                  where
                                  tst.CompanyId == (Guid)userInfo.CompanyId &&
                                  cm.UserId == userInfo.UserId
                                  select new
                                  {
                                      Id = tst.Id,
                                      Title = tst.Title,
                                      Description = tst.Description,

                                      CompanyId = tst.CompanyId,
                                      CompanyName = cmp.Name,

                                      UserId = usr.Id,
                                      UserFullName = usr.FullName,
                                      UserShortName = usr.ShortName,
                                      UserCreatedOn = tst.CreatedOn,

                                      CandidateTestId = cdt != null ? (Guid?)cdt.Id : null
                                  });

                var x = await testsQuery.ToListAsync();

                var tests = await testsQuery.GroupBy(t => new { t.Id, t.Title, t.Description, t.CompanyId, t.CompanyName, t.UserId, t.UserFullName, t.UserShortName, t.UserCreatedOn }).Select(t => new GetTestDto
                {
                    Id = t.Key.Id,
                    Title = t.Key.Title,
                    Description = t.Key.Description,

                    Company = new CompanySimpleDto
                    {
                        Id = t.Key.CompanyId,
                        Name = t.Key.CompanyName,
                    },

                    CreatedDetails = new CreatedOnDto
                    {
                        UserId = t.Key.UserId,
                        FullName = t.Key.UserFullName,
                        ShortName = t.Key.UserShortName,
                        CreatedOn = t.Key.UserCreatedOn
                    },
                    TakersCount = t.Count(ct => ct.CandidateTestId != null)
                }).ToListAsync();

                return tests;
            }
        }
    }
}
