using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Logging_part_usage;
using CarServ.Repository.Repositories.DTO.RevenueReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IPartsRepository : IGenericRepository<Part>
    {
        Task<List<PartDto>> GetAllPartsAsync();
        Task<List<Supplier>> GetAllSuppliersAsync();
        Task<List<Part>> GetLowPartsAsync();
        Task<List<Part>> GetZeroPartsAsync();
        Task<Part> GetPartByIdAsync(int partId);
        Task<List<Part>> GetPartsByPartName(string partName);
        Task<List<Part>> GetPartsByUnitPriceRange(decimal minPrice, decimal maxPrice);
        Task<List<Part>> GetPartsByExpiryDateRange(DateOnly startDate, DateOnly endDate);
        Task<List<Part>> GetPartsByWarrantyMonthsRange(int minMonths, int maxMonths);
        Task<Part> AddPartAsync(
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths);
        Task<Part> UpdatePartAsync(
            int partId,
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths);
        Task<RevenueReportDto> GenerateRevenueReport(DateTime startDate, DateTime endDate);

        Task TrackPartsUsed(PartUsageDto partsUsedDTO);
        Task UpdateServiceProgress(UpdateServiceProgressDto dto);
    }
}
