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
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.Candidates.Tests.Queries
{
    public class GetTestsAsCandidateQuery : BaseQuery<GetTestsAsCandidateQuery>
    {
        public async Task<List<GetTestDto>> GetTestsAsCandidate(UserInfo userInfo)
        {
            if (!userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var tests = (from cnd in db.Candidates
                             join pst in db.CandidateTests on cnd.Id equals pst.CandidateId
                             join tst in db.Tests on pst.TestId equals tst.Id
                             join cmp in db.Companies on tst.CompanyId equals cmp.Id
                             join usr in db.Users on tst.CreatedBy equals usr.Id
                             where
                             cnd.UserId == userInfo.UserId
                             select new GetTestDto
                             {
                                 Id = tst.Id,
                                 Title = tst.Title,
                                 Description = tst.Description,

                                 Company = new CompanySimpleDto
                                 {
                                     Id = tst.CompanyId,
                                     Name = cmp.Name,
                                 },

                                 CreatedDetails = new CreatedOnDto
                                 {
                                     UserId = usr.Id,
                                     FullName = usr.FullName,
                                     ShortName = usr.ShortName,
                                     CreatedOn = tst.CreatedOn
                                 }
                             });

                return await tests.ToListAsync();
            }
        }
    }
}
