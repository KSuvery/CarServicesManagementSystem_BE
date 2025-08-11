using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.DTO.Service_managing;
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

        public async Task<Service> CreateService(CreateServiceDto dto)
        {
            
            if (string.IsNullOrEmpty(dto.Name))
            {
                throw new ArgumentException("Service name is required.");
            }
            
            var service = new Service
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                EstimatedLaborHours = dto.EstimatedLaborHours
            };
            _context.Services.Add(service);
            await _context.SaveChangesAsync();
            return service;
        }
        public async Task<ServicePackage> CreateServicePackage(CreateServicePackageDto dto)
        {
            
            if (string.IsNullOrEmpty(dto.Name))
            {
                throw new ArgumentException("Service package name is required.");
            }
            
            var servicePackage = new ServicePackage
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Services = new List<Service>()
            };
            // Add selected services to the package
            foreach (var serviceId in dto.ServiceIds)
            {
                var service = await _context.Services.FindAsync(serviceId);
                if (service != null)
                {
                    servicePackage.Services.Add(service);
                }
            }
            _context.ServicePackages.Add(servicePackage);
            await _context.SaveChangesAsync();
            return servicePackage;
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
        public async Task<List<VehicleDto>> GetVehiclesByCustomerId(int customerId)
        {
            var vehicles = await _context.Vehicles
                .Where(v => v.CustomerId == customerId)
                .ToListAsync();
            return vehicles.Select(v => new VehicleDto
            {
                VehicleId = v.VehicleId,
                LicensePlate = v.LicensePlate,
                Make = v.Make,
                Model = v.Model,
                Year = v.Year
            }).ToList();
        }
        //  single service
        public async Task<List<PartDto>> GetPartsByServiceId(int serviceId)
        {
            var serviceParts = await _context.ServiceParts
                .Include(sp => sp.Part) 
                .Where(sp => sp.ServiceId == serviceId)
                .ToListAsync();
            return serviceParts.Select(sp => new PartDto
            {
                PartId = sp.Part.PartId,
                PartName = sp.Part.PartName,
                QuantityRequired = sp.QuantityRequired,
                UnitPrice = sp.Part.UnitPrice
            }).ToList();
        }
        // service package
        public async Task<List<PartDto>> GetPartsByPackageId(int packageId)
        {
            var serviceParts = await _context.ServiceParts
                .Include(sp => sp.Part) 
                .Where(sp => sp.Service.Packages.Any(p => p.PackageId == packageId))
                .ToListAsync();
            return serviceParts.Select(sp => new PartDto
            {
                PartId = sp.Part.PartId,
                PartName = sp.Part.PartName,
                QuantityRequired = sp.QuantityRequired,
                UnitPrice = sp.Part.UnitPrice
            }).ToList();
        }
    }
}
