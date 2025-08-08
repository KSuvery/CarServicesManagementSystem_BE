using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
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
        Task<PaginationResult<ServicePackage>> GetAllWithPaging(int pageNum, int pageSize);
        Task<ServicePackageListDto> GetAllServicePackages();
        Task<List<VehicleDto>> GetVehiclesByCustomerId(int customerId);

        Task<List<PartDto>> GetPartsByServiceId(int serviceId);

        Task<List<PartDto>> GetPartsByPackageId(int packageId);
        
    }
}
