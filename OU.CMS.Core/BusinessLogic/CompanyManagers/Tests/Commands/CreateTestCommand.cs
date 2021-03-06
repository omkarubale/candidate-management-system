﻿using FluentValidation;
using OU.CMS.Core.BusinessLogic.Base;
using OU.CMS.Core.BusinessLogic.CompanyManagers.Tests.Queries;
using OU.CMS.Domain.Contexts;
using OU.CMS.Domain.Entities;
using OU.CMS.Models.Authentication;
using OU.CMS.Models.Models.Test;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OU.CMS.Core.BusinessLogic.CompanyManagers.Tests.Commands
{
    public class CreateTestCommand : BaseCommand<CreateTestCommand>
    {
        public CreateTestDto Dto { get; set; }

        public CreateTestCommand(CreateTestDto dto)
        {
            Dto = dto;
            var validator = new Validator();
            var result = validator.Validate(this);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
        }

        public class Validator : AbstractValidator<CreateTestCommand>
        {
            public Validator()
            {
                RuleFor(i => i.Dto.Title).NotNull().NotEmpty();
                RuleFor(i => i.Dto.Description).NotNull().NotEmpty();
            }
        }

        public async Task<GetTestDto> CreateTest(UserInfo userInfo)
        {
            if (userInfo.IsCandidateLogin)
                throw new Exception("You do not have access to perform this action!");

            using (var db = new CMSContext())
            {
                var checkExistingTest = db.Tests.Any(c => c.Title == Dto.Title.Trim() && c.CompanyId == userInfo.CompanyId);
                if (checkExistingTest)
                    throw new Exception("Test with this name already exists!");

                var test = new Test
                {
                    Id = Guid.NewGuid(),
                    Title = Dto.Title.Trim(),
                    Description = Dto.Description.Trim(),
                    CompanyId = (Guid)userInfo.CompanyId,
                    CreatedBy = userInfo.UserId,
                    CreatedOn = DateTime.UtcNow,
                };

                db.Tests.Add(test);

                await db.SaveChangesAsync();

                return await new GetTestAsCompanyManagerQuery(test.Id).GetTestAsCompanyManager(userInfo);
            }
        }
    }
}
