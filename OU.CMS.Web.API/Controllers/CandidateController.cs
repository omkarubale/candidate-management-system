using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.Candidate;
using OU.CMS.Models.Models.Common;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Models.User;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.JobOpening;

namespace OU.CMS.Web.API.Controllers
{
    public class CandidateController : ApiController
    {
        private Guid myUserId = new Guid("1ff58b86-28a7-4324-bc40-518c29135f86");
        public async Task<IEnumerable<GetCandidateDto>> GetAllCandidates()
        {
            using (var db = new CMSContext())
            {
                var candidates = await (from cnd in db.Candidates
                                        join usr in db.Users on cnd.UserId equals usr.Id
                                        join cmp in db.Companies on cnd.CompanyId equals cmp.Id
                                        join jo in db.JobOpenings on cnd.JobOpeningId equals jo.Id
                                        join lc in db.Users on cnd.CreatedBy equals lc.Id
                                        select new GetCandidateDto
                                        {
                                            User = new UserSimpleDto
                                            {
                                                UserId = usr.Id,
                                                FullName = usr.FullName,
                                                ShortName = usr.ShortName,
                                                Email = usr.Email
                                            },
                                            Company = new CompanySimpleDto
                                            {
                                                Id = cmp.Id,
                                                Name = cmp.Name
                                            },
                                            JobOpening = new JobOpeningSimpleDto
                                            {
                                                Id = jo.Id,
                                                Title = jo.Title,
                                                Description = jo.Description,
                                            },
                                            CreatedDetails = new CreatedOnDto
                                            {
                                                UserId = lc.Id,
                                                FullName = lc.FullName,
                                                ShortName = lc.ShortName,
                                                CreatedOn = cnd.CreatedOn
                                            }
                                        }).ToListAsync();

                return candidates;
            }
        }

        public async Task<GetCandidateDto> GetCandidate(Guid id)
        {
            using (var db = new CMSContext())
            {
                var candidate = await (from cnd in db.Candidates
                                       join usr in db.Users on cnd.UserId equals usr.Id
                                       join cmp in db.Companies on cnd.CompanyId equals cmp.Id
                                       join jo in db.JobOpenings on cnd.JobOpeningId equals jo.Id
                                       join lc in db.Users on cnd.CreatedBy equals lc.Id
                                       where cnd.Id == id
                                       select new GetCandidateDto
                                       {
                                           User = new UserSimpleDto
                                           {
                                               UserId = usr.Id,
                                               FullName = usr.FullName,
                                               ShortName = usr.ShortName,
                                               Email = usr.Email
                                           },
                                           Company = new CompanySimpleDto
                                           {
                                               Id = cmp.Id,
                                               Name = cmp.Name
                                           },
                                           JobOpening = new JobOpeningSimpleDto
                                           {
                                               Id = jo.Id,
                                               Title = jo.Title,
                                               Description = jo.Description,
                                           },
                                           CreatedDetails = new CreatedOnDto
                                           {
                                               UserId = lc.Id,
                                               FullName = lc.FullName,
                                               ShortName = lc.ShortName,
                                               CreatedOn = cnd.CreatedOn
                                           }
                                       }).SingleOrDefaultAsync();

                if (candidate == null)
                    throw new Exception("Candidate does not exist!");

                return candidate;
            }
        }

        public async Task<GetCandidateDto> CreateCandidate(CreateCandidateDto dto)
        {
            using (var db = new CMSContext())
            {
                var checkExistingCandidate = db.Candidates.Any(c => c.UserId == dto.UserId && c.JobOpeningId == dto.JobOpeningId);
                if (checkExistingCandidate)
                    throw new Exception("Candidate has already applied for this Job Position!");

                var candidate = new Candidate
                {
                    Id = Guid.NewGuid(),
                    UserId = dto.UserId,
                    CompanyId = dto.CompanyId,
                    JobOpeningId = dto.JobOpeningId,
                    CreatedBy = myUserId, //TODO: Change to identityUser.UserId
                    CreatedOn = DateTime.UtcNow
                };

                db.Candidates.Add(candidate);

                await db.SaveChangesAsync();

                return await GetCandidate(candidate.Id);
            }
        }

        public async Task DeleteCandidate(Guid id)
        {
            using (var db = new CMSContext())
            {
                var Candidate = await db.Candidates.SingleOrDefaultAsync(c => c.Id == id);

                if (Candidate == null)
                    throw new Exception("Candidate with Id not found!");

                db.Candidates.Remove(Candidate);

                await db.SaveChangesAsync();
            }
        }
    }
}
