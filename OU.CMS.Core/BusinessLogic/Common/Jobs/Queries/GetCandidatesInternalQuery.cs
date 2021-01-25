using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.Candidate;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.JobOpening;
using OU.CMS.Models.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.Common.Jobs.Queries
{
    public class GetCandidatesInternalQuery : BaseQuery<GetCandidatesInternalQuery>
    {
        public Guid? CandidateId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? JobOpeningId { get; set; }
        public Guid? UserId { get; set; }

        public GetCandidatesInternalQuery(Guid? candidateId = null, Guid? companyId = null, Guid? jobOpeningId = null, Guid? userId = null)
        {
            CandidateId = candidateId;
            CompanyId = companyId;
            JobOpeningId = jobOpeningId;
            UserId = userId;
        }

        public async Task<List<GetCandidateDto>> GetAllCandidates()
        {
            using (var db = new CMSContext())
            {
                var isCandidateFilter = CandidateId != null && CandidateId != Guid.Empty;
                var isCompanyFilter = CompanyId != null && CompanyId != Guid.Empty;
                var isJobOpeningFilter = JobOpeningId != null && JobOpeningId != Guid.Empty;
                var isUserFilter = UserId != null && UserId != Guid.Empty;

                var candidatesQuery = from cnd in db.Candidates
                                      join usr in db.Users on cnd.UserId equals usr.Id
                                      join cmp in db.Companies on cnd.CompanyId equals cmp.Id
                                      join jo in db.JobOpenings on cnd.JobOpeningId equals jo.Id
                                      join lc in db.Users on cnd.CreatedBy equals lc.Id
                                      select new
                                      {
                                          CandidateId = cnd.Id,

                                          UserId = usr.Id,
                                          UserFullName = usr.FullName,
                                          UserShortName = usr.ShortName,
                                          UserEmail = usr.Email,

                                          CompanyId = cmp.Id,
                                          CompanyName = cmp.Name,

                                          JobOpeningId = jo.Id,
                                          JobOpeningTitle = jo.Title,
                                          JobOpeningDescription = jo.Description,

                                          CreatedDetailsUserId = lc.Id,
                                          CreatedDetailsFullName = lc.FullName,
                                          CreatedDetailsShortName = lc.ShortName,
                                          CreatedDetailsCreatedOn = cnd.CreatedOn
                                      };

                if (isCandidateFilter)
                    candidatesQuery = candidatesQuery.Where(c => c.CandidateId == CandidateId);

                if (isCompanyFilter)
                    candidatesQuery = candidatesQuery.Where(c => c.CompanyId == CompanyId);

                if (isJobOpeningFilter)
                    candidatesQuery = candidatesQuery.Where(c => c.JobOpeningId == JobOpeningId);

                if (isUserFilter)
                    candidatesQuery = candidatesQuery.Where(c => c.UserId == UserId);

                var candidates = await candidatesQuery.Select(c => new GetCandidateDto
                {
                    CandidateId = c.CandidateId,
                    User = new UserSimpleDto
                    {
                        UserId = c.UserId,
                        FullName = c.UserFullName,
                        ShortName = c.UserShortName,
                        Email = c.UserEmail
                    },
                    Company = new CompanySimpleDto
                    {
                        Id = c.CompanyId,
                        Name = c.CompanyName
                    },
                    JobOpening = new JobOpeningSimpleDto
                    {
                        Id = c.JobOpeningId,
                        Title = c.JobOpeningTitle,
                        Description = c.JobOpeningDescription,
                    },
                    CreatedDetails = new CreatedOnDto
                    {
                        UserId = c.CreatedDetailsUserId,
                        FullName = c.CreatedDetailsFullName,
                        ShortName = c.CreatedDetailsShortName,
                        CreatedOn = c.CreatedDetailsCreatedOn
                    }
                }).ToListAsync();

                return candidates;
            }
        }
    }
}
