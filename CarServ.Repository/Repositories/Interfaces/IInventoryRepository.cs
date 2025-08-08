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
    public interface IInventoryRepository : IGenericRepository<Part>
    {
        Task<List<Part>> GetAllInventoryItemsAsync();
        Task<Part> GetInventoryItemByIdAsync(int partId);
        Task<List<Part>> GetInventoryItemsByNameAsync(string partName);
        Task<Part> CreateInventoryItemAsync(
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths);
        Task<Part> UpdateInventoryItemAsync(
            int partId,
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths);
        Task<bool> RemoveInventoryItemAsync(int partId);

        //This is revenue report, i just put it here 'cause dont know wwhere to put this func
        Task<RevenueReportDto> GenerateRevenueReport(DateTime startDate, DateTime endDate);

        Task TrackPartsUsed(PartUsageDto partsUsedDTO);
        Task UpdateServiceProgress(UpdateServiceProgressDto dto);
    }
}
