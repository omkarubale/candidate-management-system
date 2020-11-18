using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.JobOpening;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using OU.CMS.Domain.Entities;
using System.Xml;

namespace OU.CMS.Web.API.Controllers
{
    public class JobPositionController : BaseSecureController
    {
        #region JobOpening
        [HttpGet]
        public async Task<List<GetJobOpeningCompanyDto>> GetAllJobOpeningsForCompany(Guid companyId)
        {
            if (!UserInfo.IsCandidateLogin && UserInfo.CompanyId != companyId)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var jobOpenings = await (from jo in db.JobOpenings
                                         join cmp in db.Companies on jo.CompanyId equals cmp.Id
                                         join cnd in db.Candidates on jo.Id equals cnd.JobOpeningId
                                         join usr in db.Users on jo.CreatedBy equals usr.Id
                                         where cmp.Id == companyId
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
                                             CandidateCount = jo.Count()
                                         }).ToListAsync();

                return jobOpenings;
            }
        }

        [HttpGet]
        public async Task<List<GetCandidateJobOpeningDto>> GetAllJobOpeningsForCandidate()
        {
            using (var db = new CMSContext())
            {
                if (!UserInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                var jobOpenings = await (from cnd in db.Candidates
                                         join jo in db.JobOpenings on cnd.JobOpeningId equals jo.Id
                                         join cmp in db.Companies on jo.CompanyId equals cmp.Id
                                         join usr in db.Users on jo.CreatedBy equals usr.Id
                                         where 
                                         cnd.UserId == UserInfo.UserId
                                         select new GetCandidateJobOpeningDto
                                         {
                                             UserId = cnd.UserId,
                                             JobOpeningId = jo.Id,
                                             CandidateId = cnd.Id,
                                             Title = jo.Title,
                                             Description = jo.Description,
                                             Salary = jo.Salary,
                                             AppliedOn = cnd.CreatedOn,
                                             Deadline = jo.Deadline,
                                             Company = new CompanySimpleDto
                                             {
                                                 Id = cmp.Id,
                                                 Name = cmp.Name
                                             },
                                             CreatedDetails = new CreatedOnDto
                                             {
                                                 UserId = usr.Id,
                                                 FullName = usr.FullName,
                                                 ShortName = usr.ShortName,
                                                 CreatedOn = jo.CreatedOn
                                             }
                                         }).ToListAsync();

                return jobOpenings;
            }
        }

        [HttpGet]
        public async Task<GetJobOpeningDto> GetJobOpening(Guid jobOpeningId)
        {
            using (var db = new CMSContext())
            {
                var jobOpening = await (from jo in db.JobOpenings
                                        join cmp in db.Companies on jo.CompanyId equals cmp.Id
                                        join usr in db.Users on jo.CreatedBy equals usr.Id
                                        where
                                        jo.Id == jobOpeningId
                                        select new GetJobOpeningDto
                                        {
                                            Id = jo.Id,
                                            Title = jo.Title,
                                            Salary = jo.Salary,
                                            Description = jo.Description,
                                            Deadline = jo.Deadline,
                                            Company = new CompanySimpleDto
                                            {
                                                Id = cmp.Id,
                                                Name = cmp.Name
                                            },
                                            CreatedDetails = new CreatedOnDto
                                            {
                                                UserId = usr.Id,
                                                FullName = usr.FullName,
                                                ShortName = usr.ShortName,
                                                CreatedOn = jo.CreatedOn
                                            }
                                        }).SingleOrDefaultAsync();

                if (UserInfo.IsCandidateLogin || UserInfo.CompanyId != jobOpening.Company.Id)
                    throw new Exception("You do not have access to perform this action!");

                if (jobOpening == null)
                    throw new Exception("JobOpening does not exist!");

                return jobOpening;
            }
        }

        [HttpPost]
        public async Task<GetJobOpeningDto> CreateJobOpening(CreateJobOpeningDto dto)
        {
            if (UserInfo.IsCandidateLogin || UserInfo.CompanyId != dto.CompanyId)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var checkExistingJobOpening = db.JobOpenings.Any(c => c.Title == dto.Title.Trim());
                if (checkExistingJobOpening)
                    throw new Exception("JobOpening with this title already exists!");

                var jobOpening = new JobOpening
                {
                    Id = Guid.NewGuid(),
                    Title = dto.Title.Trim(),
                    Description = dto.Description.Trim(),
                    Salary = dto.Salary,
                    Deadline = dto.Deadline,
                    CompanyId = dto.CompanyId,
                    CreatedBy = UserInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                db.JobOpenings.Add(jobOpening);

                await db.SaveChangesAsync();

                return await GetJobOpening(jobOpening.Id);
            }
        }

        [HttpPost]
        public async Task<GetJobOpeningDto> UpdateJobOpening(UpdateJobOpeningDto dto)
        {
            using (var db = new CMSContext())
            {
                var jobOpening = await db.JobOpenings.SingleOrDefaultAsync(c => c.Id == dto.Id);

                if (jobOpening == null)
                    throw new Exception("JobOpening with Id not found!");

                if (UserInfo.IsCandidateLogin || UserInfo.CompanyId != jobOpening.CompanyId)
                    throw new Exception("You do not have access to perform this action!");

                var checkExistingJobOpening = db.JobOpenings.Any(c => c.Title == dto.Title.Trim() && c.Id != dto.Id);
                if (checkExistingJobOpening)
                    throw new Exception("JobOpening with this name already exists!");

                jobOpening.Title = dto.Title;
                jobOpening.Description = dto.Description;
                jobOpening.Salary = dto.Salary;
                jobOpening.Deadline = dto.Deadline;

                await db.SaveChangesAsync();

                return await GetJobOpening(jobOpening.Id);
            }
        }

        [HttpDelete]
        public async Task DeleteJobOpening(Guid jobOpeningId)
        {
            using (var db = new CMSContext())
            {
                var jobOpening = await db.JobOpenings.SingleOrDefaultAsync(c => c.Id == jobOpeningId);

                if (jobOpening == null)
                    throw new Exception("JobOpening with Id not found!");

                if (UserInfo.IsCandidateLogin || UserInfo.CompanyId != jobOpening.CompanyId)
                    throw new Exception("You do not have access to perform this action!");

                var candidatesForJobOpening = await db.Candidates.Where(c => c.JobOpeningId == jobOpeningId).ToListAsync();

                db.Candidates.RemoveRange(candidatesForJobOpening);
                db.JobOpenings.Remove(jobOpening);

                await db.SaveChangesAsync();
            }
        }
        #endregion
    }
}
