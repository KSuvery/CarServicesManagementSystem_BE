using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.DTO.RevenueReport;
using CarServ.Repository.Repositories.DTO.Service_._ServicePackage;
using CarServ.Repository.Repositories.DTO.Service_managing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IPackageRepository 
    {
        Task<Service> CreateService(CreateServiceDto dto);
        Task<ServicePackage> CreateServicePackage(CreateServicePackageDto dto);
        Task<PaginationResult<ServicePackage>> GetAllWithPaging(int pageNum, int pageSize);
        Task<ServicePackageListDto> GetAllServicePackages();
        Task<ServiceListDto> GetAllServices();
        Task<List<VehicleDto>> GetVehiclesByCustomerId(int customerId);

        Task<List<PartDTO_Copy>> GetPartsByServiceId(int serviceId);

        Task<List<PartDTO_Copy>> GetPartsByPackageId(int packageId);
        Task<ServicePackage> UpdateServicePackageAsync(int packageId, UpdateServicePackageDto dto);
        Task<Service> UpdateServiceAsync(int serviceId, UpdateServiceDto dto);
        Task DeleteServiceAsync(int serviceId);
        Task DeleteServicePackageAsync(int packageId);
        Task<ServiceDto> GetService(int serviceId);
        Task<ServicePackageDto> GetServicePackage(int id);
        Task<PaginationResult<List<ServiceDto>>> GetAllServicesWithPaging(int currentPage, int pageSize);
        Task<PaginationResult<List<ServicePackageDto>>> GetAllServicePackageWithPaging(int currentPage, int pageSize);
        Task<List<DailyServicesRevenueReportDto>> GenerateDailyServicesRevenueReport(DateTime date);

    }
}
