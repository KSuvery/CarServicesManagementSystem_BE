using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
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
        Task<PaginationResult<List<PartDto>>> GetAllPartsWithPaging(int currentPage, int pageSize);
        Task<PaginationResult<List<Supplier>>> GetAllSuppliersAsync(int currentPage, int pageSize);
        Task<List<PartDto>> GetLowPartsAsync();
        Task<List<Part>> GetZeroPartsAsync();
        Task<PartDto> GetPartByIdAsync(int partId);
        Task<List<Part>> GetPartsByPartName(string partName);
        Task<List<Part>> GetPartsByUnitPriceRange(decimal minPrice, decimal maxPrice);
        Task<List<Part>> GetPartsByExpiryDateRange(DateOnly startDate, DateOnly endDate);
        Task<List<Part>> GetPartsByWarrantyMonthsRange(int minMonths, int maxMonths);
        Task<Part> CreatePartAsync(CreatePartDto dto);
        Task<Part> UpdatePartAsync(int partId, UpdatePartDto dto);
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
        Task<RevenueReportDto> GenerateRevenueReport(int month, int year);
        Task<DashboardSummaryDto> GenerateDashboardSummary(int month, int year);

        Task TrackPartsUsed(PartUsageDto partsUsedDTO);
        Task UpdateServiceProgress(UpdateServiceProgressDto dto);
        Task DeletePartAsync(int partId);
    }
}
