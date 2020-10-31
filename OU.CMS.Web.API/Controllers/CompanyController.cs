using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OU.CMS.Domain.Contexts;
using OU.CMS.Models.Models.Company;
using OU.CMS.Models.Models.Common;
using OU.CMS.Domain.Entities;
using OU.CMS.Web.API.Filters;

namespace OU.CMS.Web.API.Controllers
{
    public class CompanyController : BaseSecureController
    {
        #region Company
        [HttpGet]
        public async Task<List<GetCompanyDto>> GetAllCompanies()
        {
            using (var db = new CMSContext())
            {
                var companies = await (from cmp in db.Companies
                                       join usr in db.Users on cmp.CreatedBy equals usr.Id
                                       select new GetCompanyDto
                                       {
                                           Id = cmp.Id,
                                           Name = cmp.Name,
                                           CreatedDetails = new CreatedOnDto
                                           {
                                               UserId = usr.Id,
                                               FullName = usr.FullName,
                                               ShortName = usr.ShortName,
                                               CreatedOn = cmp.CreatedOn
                                           }
                                       }).ToListAsync();

                return companies;
            }
        }
        
        [HttpGet]
        public async Task<GetCompanyDto> GetCompany(Guid companyId)
        {
            using (var db = new CMSContext())
            {
                var company = await (from cmp in db.Companies
                                     join usr in db.Users on cmp.CreatedBy equals usr.Id
                                     where cmp.Id == companyId
                                     select new GetCompanyDto
                                     {
                                         Id = cmp.Id,
                                         Name = cmp.Name,
                                         CreatedDetails = new CreatedOnDto
                                         {
                                             UserId = usr.Id,
                                             FullName = usr.FullName,
                                             ShortName = usr.ShortName,
                                             CreatedOn = cmp.CreatedOn
                                         }
                                     }).SingleOrDefaultAsync();

                if (company == null)
                    throw new Exception("Company does not exist!");

                return company;
            }
        }

        [HttpPost]
        public async Task<GetCompanyDto> CreateCompany(CreateCompanyDto dto)
        {
            using (var db = new CMSContext())
            {
                var checkExistingCompany = db.Companies.Any(c => c.Name == dto.Name.Trim());
                if (checkExistingCompany)
                    throw new Exception("Company with this name already exists!");

                var company = new Company
                {
                    Id = Guid.NewGuid(),
                    Name = dto.Name.Trim(),
                    CreatedBy = UserInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                db.Companies.Add(company);

                await db.SaveChangesAsync();

                return new GetCompanyDto()
                {
                    Id = company.Id,
                    Name = company.Name,
                    CreatedDetails = new CreatedOnDto
                    {
                        UserId = company.CreatedBy,
                        FullName = UserInfo.FullName,
                        ShortName = UserInfo.ShortName,
                        CreatedOn = company.CreatedOn
                    }
                };
            }
        }

        [HttpPost]
        public async Task<GetCompanyDto> SaveCompany(SaveCompanyDto dto)
        {
            using (var db = new CMSContext())
            {
                Company company;
                var isNew = dto.Id == Guid.Empty;
                var checkExistingCompany = db.Companies.Any(c => c.Name == dto.Name.Trim() && (isNew || c.Id != dto.Id));
                if (checkExistingCompany)
                    throw new Exception("Company with this name already exists!");

                if (isNew)
                {
                    company = new Company
                    {
                        Id = Guid.NewGuid(),
                        CreatedBy = UserInfo.UserId,
                        CreatedOn = DateTime.UtcNow
                    };
                }
                else
                {
                    company = await db.Companies.SingleOrDefaultAsync(c => c.Id == dto.Id);
                    if (company == null)
                        throw new Exception("Company with Id not found!");
                }
                
                company.Name = dto.Name.Trim();

                if(isNew)
                    db.Companies.Add(company);

                await db.SaveChangesAsync();

                return new GetCompanyDto()
                {
                    Id = company.Id,
                    Name = company.Name,
                    CreatedDetails = new CreatedOnDto
                    {
                        UserId = company.CreatedBy,
                        FullName = UserInfo.FullName,
                        ShortName = UserInfo.ShortName,
                        CreatedOn = company.CreatedOn
                    }
                };
            }
        }

        [HttpDelete]
        public async Task DeleteCompany(Guid companyId)
        {
            using (var db = new CMSContext())
            {
                var company = await db.Companies.Include(c => c.JobOpenings).Include(c => c.CompanyManagements).Include(c => c.CompanyManagementInvites).SingleOrDefaultAsync(c => c.Id == companyId);

                if (company == null)
                    throw new Exception("Company with Id not found!");

                var companyCandidates = await db.Candidates.AnyAsync(c => c.CompanyId == companyId);

                if (!companyCandidates)
                    throw new Exception("Company cannot be deleted as candidates have registered for this company's job Openings!");

                db.CompanyManagementInvites.RemoveRange(company.CompanyManagementInvites);
                db.CompanyManagements.RemoveRange(company.CompanyManagements);
                db.JobOpenings.RemoveRange(company.JobOpenings);

                db.Companies.Remove(company);

                await db.SaveChangesAsync();
            }
        }
        #endregion

        #region CompanyManagement
        [HttpPost]
        public async Task DeleteCompanyManagement(DeleteCompanyManagementDto dto)
        {
            using (var db = new CMSContext())
            {
                var companyManagement = await db.CompanyManagements.SingleOrDefaultAsync(c => c.CompanyId == dto.CompanyId && c.UserId == dto.UserId);

                if (companyManagement == null)
                    throw new Exception("This User is not a part of this Company!");

                db.CompanyManagements.Remove(companyManagement);

                await db.SaveChangesAsync();
            }
        }

        [HttpPut]
        public async Task CreateCompanyManagementInvite(CreateCompanyManagementInviteDto dto)
        {
            using (var db = new CMSContext())
            {
                var checkExistingInvite = await db.CompanyManagementInvites.AnyAsync(c => c.Email == dto.Email.Trim() && c.CompanyId == dto.CompanyId);
                if (checkExistingInvite)
                    throw new Exception("Company Management Invite for this Email already exists!");

                var checkExistingManagement = await (from cm in db.CompanyManagements
                                                     join usr in db.Users on cm.UserId equals usr.Id
                                                     where
                                                     cm.CompanyId == dto.CompanyId &&
                                                     usr.Email == dto.Email.Trim()
                                                     select cm).AnyAsync();
                if (checkExistingManagement)
                    throw new Exception("This User is already a part of this company!");

                var companyManagementInvite = new CompanyManagementInvite
                {
                    Id = Guid.NewGuid(),
                    CompanyId = dto.CompanyId,
                    Email = dto.Email,
                    IsInviteForAdmin = dto.IsInviteForAdmin,
                    CreatedBy = UserInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                db.CompanyManagementInvites.Add(companyManagementInvite);

                await db.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task AcceptCompanyManagementInvite(AcceptCompanyManagementInviteDto dto)
        {
            using (var db = new CMSContext())
            {
                //TODO: change to identityUser.Email
                var companyManagementInvite = await db.CompanyManagementInvites.SingleOrDefaultAsync(c => c.Email == UserInfo.Email && c.CompanyId == dto.CompanyId);
                if (companyManagementInvite == null)
                    throw new Exception("Company Management Invite for this User doesn't exist!");

                db.CompanyManagementInvites.Remove(companyManagementInvite);

                var companyManagement = new CompanyManagement
                {
                    Id = Guid.NewGuid(),
                    CompanyId = dto.CompanyId,
                    UserId = UserInfo.UserId,
                    IsAdmin = companyManagementInvite.IsInviteForAdmin,
                    CreatedBy = UserInfo.UserId,
                    CreatedOn = DateTime.UtcNow
                };

                db.CompanyManagements.Add(companyManagement);

                await db.SaveChangesAsync();
            }
        }

        [HttpPost]
        public async Task RevokeCompanyManagementInvite(RevokeCompanyManagementInviteDto dto)
        {
            using (var db = new CMSContext())
            {
                var companyManagementInvite = await db.CompanyManagementInvites.SingleAsync(c => c.Email == dto.Email.Trim() && c.CompanyId == dto.CompanyId);
                if (companyManagementInvite == null)
                    throw new Exception("Company Management Invite for this Email doesn't exist!");

                var checkExistingManagement = await (from cm in db.CompanyManagements
                                                     join usr in db.Users on cm.UserId equals usr.Id
                                                     where
                                                     cm.CompanyId == dto.CompanyId &&
                                                     usr.Email == dto.Email.Trim()
                                                     select cm).AnyAsync();
                if (checkExistingManagement)
                    throw new Exception("This User is already a part of this company!");

                db.CompanyManagementInvites.Remove(companyManagementInvite);

                await db.SaveChangesAsync();
            }
        }
        #endregion
    }
}
