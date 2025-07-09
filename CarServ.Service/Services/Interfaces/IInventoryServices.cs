using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.RevenueReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface IInventoryServices
    {
        Task<List<Inventory>> GetAllInventoryItemsAsync();
        Task<Inventory> GetInventoryItemByIdAsync(int partId);
        Task<List<Inventory>> GetInventoryItemsByNameAsync(string partName);
        Task<Inventory> CreateInventoryItemAsync(
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths);
        Task<Inventory> UpdateInventoryItemAsync(
            int partId,
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths);
        Task<bool> RemoveInventoryItemAsync(int partId);
        
        //This is revenue report, i just put it here 'cause dont know wwhere to put this func
        Task<RevenueReportDto> GenerateRevenueReport(DateTime startDate, DateTime endDate);
    }
}
