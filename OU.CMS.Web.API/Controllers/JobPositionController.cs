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
    public class JobPositionController : ApiController
    {
        private Guid myUserId = new Guid("1ff58b86-28a7-4324-bc40-518c29135f86");
        //private string myEmail = "oubale@gmail.com";

        #region JobOpening
        public async Task<List<GetJobOpeningCompanyDto>> GetAllJobOpeningsForCompany(Guid companyId)
        {
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

        public async Task<List<GetJobOpeningDto>> GetAllJobOpeningsForCandidate(Guid candidateId)
        {
            using (var db = new CMSContext())
            {
                var jobOpenings = await (from jo in db.JobOpenings
                                         join cmp in db.Companies on jo.CompanyId equals cmp.Id
                                         join cnd in db.Candidates on jo.Id equals cnd.JobOpeningId
                                         join usr in db.Users on jo.CreatedBy equals usr.Id
                                         where cnd.Id == candidateId
                                         select new GetJobOpeningDto
                                         {
                                             Id = jo.Id,
                                             Title = jo.Title,
                                             Description = jo.Description,
                                             Salary = jo.Salary,
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

                if (jobOpening == null)
                    throw new Exception("JobOpening does not exist!");

                return jobOpening;
            }
        }

        public async Task<GetJobOpeningDto> CreateJobOpening(CreateJobOpeningDto dto)
        {
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
                    CreatedBy = myUserId, //TODO: Change to identityUser.UserId
                    CreatedOn = DateTime.UtcNow
                };

                db.JobOpenings.Add(jobOpening);

                await db.SaveChangesAsync();

                return await GetJobOpening(jobOpening.Id);
            }
        }

        public async Task<GetJobOpeningDto> UpdateJobOpening(UpdateJobOpeningDto dto)
        {
            using (var db = new CMSContext())
            {
                var jobOpening = await db.JobOpenings.SingleOrDefaultAsync(c => c.Id == dto.Id);
                if (jobOpening == null)
                    throw new Exception("JobOpening with Id not found!");

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

        public async Task DeleteJobOpening(Guid jobOpeningId)
        {
            using (var db = new CMSContext())
            {
                var jobOpening = await db.JobOpenings.SingleOrDefaultAsync(c => c.Id == jobOpeningId);

                if (jobOpening == null)
                    throw new Exception("JobOpening with Id not found!");

                var candidatesForJobOpening = await db.Candidates.Where(c => c.JobOpeningId == jobOpeningId).ToListAsync();

                db.Candidates.RemoveRange(candidatesForJobOpening);
                db.JobOpenings.Remove(jobOpening);

                await db.SaveChangesAsync();
            }
        }
        #endregion
    }
}
