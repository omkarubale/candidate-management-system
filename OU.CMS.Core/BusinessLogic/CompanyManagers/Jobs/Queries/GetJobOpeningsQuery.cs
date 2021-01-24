using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.JobOpening;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Queries
{
    public class GetJobOpeningsQuery : BaseQuery<GetJobOpeningsQuery>
    {
        public async Task<List<GetJobOpeningCompanyDto>> GetJobOpenings(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var jobOpenings = await (from jo in db.JobOpenings
                                         join cmp in db.Companies on jo.CompanyId equals cmp.Id
                                         join cnd in db.Candidates on jo.Id equals cnd.JobOpeningId into candidateTemp
                                         from cnd in candidateTemp.DefaultIfEmpty()
                                         join usr in db.Users on jo.CreatedBy equals usr.Id into candidateUserTemp
                                         from usr in candidateUserTemp.DefaultIfEmpty()
                                         where cmp.Id == userInfo.CompanyId
                                         group cnd by new { jo.Id, jo.Title, jo.Description, jo.Salary, jo.Deadline, jo.CreatedOn, CompanyId = cmp.Id, cmp.Name, UserId = usr.Id, usr.FullName, usr.ShortName } into jo
                                         select new GetJobOpeningCompanyDto
                                         {
                                             Id = jo.Key.Id,
                                             Title = jo.Key.Title,
                                             Description = jo.Key.Description,
                                             Salary = jo.Key.Salary,
                                             Deadline = jo.Key.Deadline,
                                             Company = new CompanySimpleDto
                                             {
                                                 Id = jo.Key.UserId,
                                                 Name = jo.Key.Name
                                             },
                                             CreatedDetails = new CreatedOnDto
                                             {
                                                 UserId = jo.Key.CompanyId,
                                                 FullName = jo.Key.FullName,
                                                 ShortName = jo.Key.ShortName,
                                                 CreatedOn = jo.Key.CreatedOn
                                             },
                                             CandidateCount = jo.Count(j => j.UserId != null)
                                         }).ToListAsync();

                return jobOpenings;
            }
        }
    }
}
