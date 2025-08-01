using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IPartsRepository : IGenericRepository<Parts>
    {
        Task<List<Parts>> GetAllPartsAsync();
        Task<Parts> GetPartByIdAsync(int partId);
        Task<List<Parts>> GetPartsByPartName(string partName);
        Task<List<Parts>> GetPartsByUnitPriceRange(decimal minPrice, decimal maxPrice);
        Task<List<Parts>> GetPartsByExpiryDateRange(DateOnly startDate, DateOnly endDate);
        Task<List<Parts>> GetPartsByWarrantyMonthsRange(int minMonths, int maxMonths);
        Task<Parts> AddPartAsync(
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths);
        Task<Parts> UpdatePartAsync(
            int partId,
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths);
    }
}
