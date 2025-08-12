using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.DTO.Service_managing;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services
{
    public class PackageServices : IPackageServices
    {
        private readonly IPackageRepository _repository;
        public PackageServices(IPackageRepository repository)
        {
            _repository = repository;
        }

        public async Task<Domain.Entities.Service> CreateService(CreateServiceDto dto)
        {
            return await _repository.CreateService(dto);
        }

        public async Task<ServicePackage> CreateServicePackage(CreateServicePackageDto dto)
        {
            return await _repository.CreateServicePackage(dto);
        }

        public async Task<ServicePackageListDto> GetAllServicePackages()
        {
            return await _repository.GetAllServicePackages();
        }        

        public async Task<PaginationResult<ServicePackage>> GetAllWithPaging(int pageNum, int pageSize)
        {
            return await _repository.GetAllWithPaging(pageNum, pageSize);
        }

        public async Task<List<PartDto>> GetPartsByPackageId(int packageId)
        {
            return await _repository.GetPartsByPackageId(packageId);
        }

        public async Task<List<PartDto>> GetPartsByServiceId(int serviceId)
        {
            return await _repository.GetPartsByServiceId(serviceId);
        }

        public async Task<List<VehicleDto>> GetVehiclesByCustomerId(int customerId)
        {
            return await _repository.GetVehiclesByCustomerId(customerId);
        }
    }
}
