using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.DTO.Service_._ServicePackage;
using CarServ.Repository.Repositories.DTO.Service_managing;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarServ.Repository.Repositories.PackageRepository;

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
                Discount = package.Discount,
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

        public async Task<ServiceListDto> GetAllServices()
        {
            var services = await _context.Services
                       .Include(sp => sp.ServiceParts)
                       .ThenInclude(sp => sp.Part)
                       .ToListAsync();
            var serviceDtos = services.Select(service => new ServiceDto
            {
                ServiceId = service.ServiceId,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price ?? 0,
                Parts = service.ServiceParts.Select(part => new PartDto
                {
                    PartId = part.PartId,
                    PartName = part.Part?.PartName ?? "Unknown",
                    QuantityRequired = part.QuantityRequired,
                    UnitPrice = part.Part?.UnitPrice ?? 0
                }).ToList()
            }).ToList();
            return new ServiceListDto
            {
                Services = serviceDtos,
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

        public async Task<ServiceDto> GetService(int serviceId)
        {
            var service = await _context.Services
                .Include(p => p.ServiceParts)
                .ThenInclude(sp => sp.Part)
                .FirstOrDefaultAsync(p => p.ServiceId == serviceId);

            if (service == null)
            {
                throw new Exception("Service not found.");
            }
            var serviceDto = new ServiceDto
            {
                ServiceId = service.ServiceId,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price ?? 0,
                Parts = service.ServiceParts.Select(part => new PartDto
                {
                    PartId = part.PartId,
                    PartName = part.Part?.PartName ?? "Unknown",
                    QuantityRequired = part.QuantityRequired,
                    UnitPrice = part.Part?.UnitPrice ?? 0
                }).ToList()
            };
            return serviceDto;

        }

        public async Task<ServicePackageDto> GetServicePackage(int id)
        {
            var package = await _context.ServicePackages
                .Include(sp => sp.Services)
                .FirstOrDefaultAsync(p => p.PackageId == id);

            if (package == null)
            {
                throw new Exception("Service not found.");
            }
            var packageDto = new ServicePackageDto
            {
                PackageId = package.PackageId,
                Name = package.Name,
                Description = package.Description,
                Price = package.Price,
                Discount = package.Discount,
                Services = package.Services.Select(service => new ServiceDto
                {
                    ServiceId = service.ServiceId,
                    Name = service.Name,
                    Description = service.Description,
                    EstimatedLaborHours = service.EstimatedLaborHours
                }).ToList()
            };
            return packageDto;         

        }

        public async Task<Service> UpdateServiceAsync(int serviceId, UpdateServiceDto dto)
            {
                var service = await _context.Services
                    .Include(s => s.ServiceParts) 
                    .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

                if (service == null)
                {
                    throw new Exception("Service not found.");
                }

                // Update service properties
                service.Name = dto.Name;
                service.Description = dto.Description;
                service.Price = dto.Price;
                service.EstimatedLaborHours = dto.EstimatedLaborHours;

              
                service.ServiceParts.Clear(); 

                foreach (var partDto in dto.ServiceParts)
                {
                    var servicePart = new ServicePart
                    {
                        ServiceId = serviceId,
                        PartId = partDto.PartId,
                        QuantityRequired = partDto.QuantityRequired
                    };
                    service.ServiceParts.Add(servicePart);
                }

                await _context.SaveChangesAsync();
                return service;
            }        

        public async Task<ServicePackage> UpdateServicePackageAsync(int packageId, UpdateServicePackageDto dto)
            {
                var package = await _context.ServicePackages
                    .Include(p => p.Services)
                    .FirstOrDefaultAsync(p => p.PackageId == packageId);

                if (package == null)
                {
                    throw new Exception("Service package not found.");
                }

                // Update package properties
                package.Name = dto.Name;
                package.Description = dto.Description;
                package.Price = dto.Price;
                package.Discount = dto.Discount;

                
                package.Services.Clear(); 

                foreach (var serviceId in dto.ServiceIds)
                {
                    var service = await _context.Services.FindAsync(serviceId);
                    if (service != null)
                    {
                        package.Services.Add(service);
                    }
                }

                await _context.SaveChangesAsync();
                return package;
            }
            public async Task DeleteServiceAsync(int serviceId)
            {
                var service = await _context.Services
                    .Include(s => s.AppointmentServices)
                        .ThenInclude(a => a.Appointment)
                        .ThenInclude(a => a.ServiceProgresses)
                    .FirstOrDefaultAsync(s => s.ServiceId == serviceId);
                if (service == null)
                {
                    throw new Exception("Service not found.");
                }
          
                var hasInProgressAppointments = service.AppointmentServices
                    .Any(a => a.Appointment.ServiceProgresses
                        .Any(sp => sp.Status != "Completed"));
                if (hasInProgressAppointments)
                {
                    throw new Exception("Cannot delete service that is associated with appointments that are not completed.");
                }
            
                _context.Services.Remove(service);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteServicePackageAsync(int packageId)
            {
                var package = await _context.ServicePackages
                .Include(p => p.Appointments)
                    .ThenInclude(a => a.ServiceProgresses) 
                .FirstOrDefaultAsync(p => p.PackageId == packageId);
                    if (package == null)
                    {
                        throw new Exception("Service package not found.");
                    }
                 
                    var hasInProgressAppointments = package.Appointments
                        .Any(a => a.ServiceProgresses
                            .Any(sp => sp.Status != "Completed"));
                    if (hasInProgressAppointments)
                    {
                        throw new Exception("Cannot delete service package that is associated with appointments that are not completed.");
                    }
            
                    _context.ServicePackages.Remove(package);
                    await _context.SaveChangesAsync();
            }        

    }
}
