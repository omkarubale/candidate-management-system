using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Common;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.JobOpening;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Jobs.Queries
{
    public class GetJobOpeningQuery : BaseQuery<GetJobOpeningQuery>
    {
        public Guid JobOpeningId { get; set; }

        public GetJobOpeningQuery(Guid jobOpeningId)
        {
            JobOpeningId = jobOpeningId;

            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<GetJobOpeningQuery>
        {
            public Validator()
            {
                RuleFor(i => i.JobOpeningId).NotNull().NotEmpty();
            }
        }

        public async Task<GetJobOpeningDto> GetJobOpening(UserInfo userInfo)
        {
            using (var db = new CMSContext())
            {
                if (userInfo.IsCandidateLogin)
                    throw new Exception("You do not have access to perform this action!");

                var jobOpening = await (from jo in db.JobOpenings
                                        join cmp in db.Companies on jo.CompanyId equals cmp.Id
                                        join usr in db.Users on jo.CreatedBy equals usr.Id
                                        where
                                        jo.Id == JobOpeningId
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

                if (userInfo.CompanyId != jobOpening.Company.Id)
                    throw new Exception("You do not have access to perform this action!");

                return jobOpening;
            }
        }
    }
}
