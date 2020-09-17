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

namespace OU.CMS.Web.API.Controllers
{
    public class CompanyController : ApiController
    {
        private Guid myUserId = new Guid("1ff58b86-28a7-4324-bc40-518c29135f86");
        public async Task<IEnumerable<GetCompanyDto>> GetAllCompanies()
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

        public async Task<GetCompanyDto> GetCompany(Guid id)
        { 
            using (var db = new CMSContext())
            {
                var company = await (from cmp in db.Companies
                                     join usr in db.Users on cmp.CreatedBy equals usr.Id
                                     where cmp.Id == id
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
                    CreatedBy = myUserId, //TODO: Change to identityUser.UserId
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
                        FullName = "Omkar Ubale", //TODO: Change to identityUser.FullName
                        ShortName = "OU", //TODO: Change to identityUser.ShortName
                        CreatedOn = company.CreatedOn
                    }
                };
            }
        }

        public async Task<GetCompanyDto> UpdateCompany(UpdateCompanyDto dto)
        {
            using (var db = new CMSContext())
            {
                var company = await db.Companies.SingleOrDefaultAsync(c => c.Id == dto.Id);
                if(company == null)
                    throw new Exception("Company with Id not found!");

                var checkExistingCompany = db.Companies.Any(c => c.Name == dto.Name.Trim());
                if (checkExistingCompany)
                    throw new Exception("Company with this name already exists!");

                company.Name = dto.Name;

                await db.SaveChangesAsync();

                return new GetCompanyDto()
                {
                    Id = company.Id,
                    Name = company.Name,
                    CreatedDetails = new CreatedOnDto
                    {
                        UserId = company.CreatedBy,
                        FullName = "Omkar Ubale", //TODO: Change to identityUser.FullName
                        ShortName = "OU", //TODO: Change to identityUser.ShortName
                        CreatedOn = company.CreatedOn
                    }
                };
            }
        }

        public async Task DeleteCompany(Guid id)
        {
            using (var db = new CMSContext())
            {
                var company = await db.Companies.SingleOrDefaultAsync(c => c.Id == id);

                if (company == null)
                    throw new Exception("Company with Id not found!");

                db.Companies.Remove(company);

                await db.SaveChangesAsync();
            }
        }
    }
}
