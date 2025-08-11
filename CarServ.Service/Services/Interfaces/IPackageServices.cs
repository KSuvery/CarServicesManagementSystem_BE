using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.DTO.Service_managing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services.Interfaces
{
    public interface IPackageServices
    {
        Task<Service> CreateService(CreateServiceDto dto);
        Task<ServicePackage> CreateServicePackage(CreateServicePackageDto dto);
        Task<PaginationResult<ServicePackage>> GetAllWithPaging(int pageNum, int pageSize);
        Task<ServicePackageListDto> GetAllServicePackages();
        Task<List<VehicleDto>> GetVehiclesByCustomerId(int customerId);
        Task<List<PartDto>> GetPartsByServiceId(int serviceId);

        Task<List<PartDto>> GetPartsByPackageId(int packageId);
    }
}
