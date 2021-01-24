using FluentValidation;
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

namespace OU.CMS.Core.BusinessLogic.Candidates.Jobs.Queries
{
    public class GetJobOpeningsForCandidateQuery : BaseQuery<GetJobOpeningsForCandidateQuery>
    {
        public Guid? JobOpeningId { get; set; }

        public GetJobOpeningsForCandidateQuery(Guid? jobOpeningId)
        {
            JobOpeningId = jobOpeningId;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<GetJobOpeningsForCandidateQuery>
        {
            public Validator()
            {
                RuleFor(i => i.JobOpeningId).NotEmpty().When(i => i.JobOpeningId != null);
            }
        }

        public async Task<List<GetCandidateJobOpeningDto>> GetJobOpeningsForCandidate(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                if (!userInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                var jobOpeningsQuery = (from jo in db.JobOpenings
                                        join cmp in db.Companies on jo.CompanyId equals cmp.Id
                                        join usr in db.Users on jo.CreatedBy equals usr.Id
                                        join cnd in db.Candidates on jo.Id equals cnd.JobOpeningId into candidateTemp
                                        from cnd in candidateTemp.DefaultIfEmpty()
                                        where
                                        jo.Deadline > DateTime.UtcNow &&
                                        (cnd.Id == null || cnd.UserId == userInfo.UserId)
                                        select new GetCandidateJobOpeningDto
                                        {
                                            // Job Opening
                                            JobOpeningId = jo.Id,
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
                                            },

                                            // Candidate
                                            CandidateId = cnd.Id != null ? (Guid?)cnd.Id : null,
                                            AppliedOn = cnd.Id != null ? (DateTime?)cnd.CreatedOn : null,
                                        });

                if (JobOpeningId != null)
                    jobOpeningsQuery = jobOpeningsQuery.Where(j => j.JobOpeningId == JobOpeningId);

                return await jobOpeningsQuery.ToListAsync();
            }
        }
    }
}
