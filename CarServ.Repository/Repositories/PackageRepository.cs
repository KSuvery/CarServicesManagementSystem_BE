using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories
{
    public class PackageRepository : GenericRepository<ServicePackage>, IPackageRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public PackageRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ServicePackageListDto> GetAllServicePackages()
        {
            var packages = await _context.ServicePackages
        .Include(sp => sp.Services) 
        .ToListAsync();
            var packageDtos = packages.Select(package => new ServicePackageDto
            {
                PackageId = package.PackageId,
                Name = package.Name,
                Description = package.Description,
                Price = package.Price,
                Services = package.Services.Select(service => new ServiceDto
                {
                    ServiceId = service.ServiceId,
                    Name = service.Name,
                    Description = service.Description,
                    EstimatedLaborHours = service.EstimatedLaborHours
                }).ToList()
            }).ToList();
            return new ServicePackageListDto
            {
                Packages = packageDtos,
                CurrentDate = DateTime.Now
            };


        }


        public Task<PaginationResult<ServicePackage>> GetAllWithPaging(int pageNum, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
