using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services
{
    public class PackageServices : IPackageServices
    {
        private readonly IPackageRepository _repository;
        public PackageServices(IPackageRepository repository)
        {
            _repository = repository;
        }
        public async Task<ServicePackageListDto> GetAllServicePackages()
        {
            return await _repository.GetAllServicePackages();
        }        

        public Task<PaginationResult<ServicePackage>> GetAllWithPaging(int pageNum, int pageSize)
        {
            throw new NotImplementedException();
        }
        public async Task<List<VehicleDto>> GetVehiclesByCustomerId(int customerId)
        {
            return await _repository.GetVehiclesByCustomerId(customerId);
        }
    }
}
