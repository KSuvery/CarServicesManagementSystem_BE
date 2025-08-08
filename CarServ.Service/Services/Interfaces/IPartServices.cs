using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Logging_part_usage;
using CarServ.Repository.Repositories.DTO.RevenueReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface IPartServices
    {
        Task<List<Part>> GetAllPartItemsAsync();
        Task<Part> GetPartItemByIdAsync(int partId);
        Task<List<Part>> GetPartItemsByNameAsync(string partName);
        Task<Part> CreatePartItemAsync(
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths);
        Task<Part> UpdatePartItemAsync(
            int partId,
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths);
        Task<bool> RemovePartItemAsync(int partId);
        
        //This is revenue report, i just put it here 'cause dont know wwhere to put this func
        Task<RevenueReportDto> GenerateRevenueReport(DateTime startDate, DateTime endDate);
        void TrackPartsUsed(PartUsageDto partUsage);
        Task UpdateServiceProgress(UpdateServiceProgressDto dto);




    }
}
