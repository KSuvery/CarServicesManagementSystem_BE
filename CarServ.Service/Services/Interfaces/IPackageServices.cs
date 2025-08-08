using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface IPackageServices
    {
        Task<PaginationResult<ServicePackages>> GetAllWithPaging(int pageNum, int pageSize);
        Task<ServicePackageListDto> GetAllServicePackages();

    }
}
