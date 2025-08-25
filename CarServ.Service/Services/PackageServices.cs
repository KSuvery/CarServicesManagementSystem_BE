using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.DTO.RevenueReport;
using CarServ.Repository.Repositories.DTO.Service_._ServicePackage;
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

        public async Task DeleteServiceAsync(int serviceId)
        {
            await _repository.DeleteServiceAsync(serviceId);
        }

        public async Task DeleteServicePackageAsync(int packageId)
        {
            await _repository.DeleteServicePackageAsync(packageId);
        }

        public async Task<ServicePackageListDto> GetAllServicePackages()
        {
            return await _repository.GetAllServicePackages();
        }

        public async Task<PaginationResult<List<ServicePackageDto>>> GetAllServicePackageWithPaging(int currentPage, int pageSize)
        {
            return await _repository.GetAllServicePackageWithPaging(currentPage, pageSize);
        }

        public async Task<ServiceListDto> GetAllServices()
        {
            return await _repository.GetAllServices();
        }

        public async Task<PaginationResult<List<ServiceDto>>> GetAllServicesWithPaging(int currentPage, int pageSize)
        {
            return await _repository.GetAllServicesWithPaging(currentPage, pageSize);
        }

        public async Task<PaginationResult<ServicePackage>> GetAllWithPaging(int pageNum, int pageSize)
        {
            return await _repository.GetAllWithPaging(pageNum, pageSize);
        }

        public async Task<List<PartDTO_Copy>> GetPartsByPackageId(int packageId)
        {
            return await _repository.GetPartsByPackageId(packageId);
        }

        public async Task<List<PartDTO_Copy>> GetPartsByServiceId(int serviceId)
        {
            return await _repository.GetPartsByServiceId(serviceId);
        }

        public async Task<ServiceDto> GetService(int serviceId)
        {
            return await _repository.GetService(serviceId); 
        }

        public async Task<ServicePackageDto> GetServicePackage(int id)
        {
            return await _repository.GetServicePackage(id);
        }

        public async Task<List<VehicleDto>> GetVehiclesByCustomerId(int customerId)
        {
            return await _repository.GetVehiclesByCustomerId(customerId);
        }

        public async Task<Domain.Entities.Service> UpdateServiceAsync(int serviceId, UpdateServiceDto dto)
        {
            return await _repository.UpdateServiceAsync(serviceId, dto);
        }

        public async Task<ServicePackage> UpdateServicePackageAsync(int packageId, UpdateServicePackageDto dto)
        {
            return await _repository.UpdateServicePackageAsync(packageId, dto);
        }

        public async Task<List<DailyServicesRevenueReportDto>> GenerateDailyServicesRevenueReport(DateTime date)
        {
            return await _repository.GenerateDailyServicesRevenueReport(date);
        }
    }
}
