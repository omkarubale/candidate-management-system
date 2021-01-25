using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Core.BusinessLogic.CompanyManagers.Tests.Queries;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Test;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Tests.Commands
{
    public class UpdateTestCommand : BaseCommand<UpdateTestCommand>
    {
        public UpdateTestDto Dto { get; set; }

        public UpdateTestCommand(UpdateTestDto dto)
        {
            Dto = dto;
            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<UpdateTestCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.Id).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Title).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Description).NotNull().NotEmpty();
            }
        }

        public async Task<GetTestDto> UpdateTest(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var test = await db.Tests.SingleOrDefaultAsync(c => c.Id == Dto.Id && c.CompanyId == userInfo.CompanyId);
                if (test == null)
                    throw new Exception("Test with Id not found!");

                var checkExistingTest = db.Tests.Any(c => c.Title == Dto.Title.Trim() && c.CompanyId == userInfo.CompanyId && c.Id != Dto.Id);
                if (checkExistingTest)
                    throw new Exception("Test with this title already exists!");

                test.Title = Dto.Title;
                test.Description = Dto.Description;

                await db.SaveChangesAsync();

                return await new GetTestAsCompanyManagerQuery(test.Id).GetTestAsCompanyManager(userInfo);
            }
        }
    }
}
