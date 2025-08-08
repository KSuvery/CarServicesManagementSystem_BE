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
    public class PackageRepository : GenericRepository<ServicePackages>, IPackageRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public PackageRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ServicePackageListDto> GetAllServicePackages()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);

            // PLEASE FIX YOUR CODE BLOCK BELOW (NEW DB DOESN'T HAVE "PromotionServicePackages") AND I DON'T KNOW WHAT TO PUT IN

            var packages = await _context.ServicePackages.Include(c => c.Appointments)
                .Select(sp => new ServicePackageDto
                {
                    PackageID = sp.PackageId,
                    Name = sp.Name,
                    Description = sp.Description,
                    Price = (decimal)sp.Price,
                    //DiscountedPrice = _context.Promotions
                    //.Where(p => p.StartDate <= currentDate &&
                    //           p.EndDate >= currentDate &&
                    //           p.PromotionServicePackages.Any(psp => psp.PackageID == sp.PackageId))
                    //.Select(p => sp.Price * (1 - (p.DiscountPercentage / 100)))
                    //.FirstOrDefault(),
                    //PromotionTitle = _context.Promotions
                    //.Where(p => p.StartDate <= currentDate &&
                    //           p.EndDate >= currentDate &&
                    //           p.PromotionServicePackages.Any(psp => psp.PackageID == sp.PackageId))
                    //.Select(p => p.Title)
                    //.FirstOrDefault()
                })
                .ToListAsync();

            return new ServicePackageListDto
            {
                Packages = packages,
                CurrentDate = DateTime.Now
            };


        }


        public Task<PaginationResult<ServicePackages>> GetAllWithPaging(int pageNum, int pageSize)
        {
            throw new NotImplementedException();
        }
    }
}
