using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.RevenueReport;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services
{
    public class InventoryServices : IInventoryServices
    {
        private readonly IInventoryRepository _inventoryRepository;

        public InventoryServices(IInventoryRepository inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<List<Inventory>> GetAllInventoryItemsAsync()
        {
            return await _inventoryRepository.GetAllInventoryItemsAsync();
        }

        public async Task<Inventory> GetInventoryItemByIdAsync(int partId)
        {
            return await _inventoryRepository.GetInventoryItemByIdAsync(partId);
        }

        public async Task<List<Inventory>> GetInventoryItemsByNameAsync(string partName)
        {
            return await _inventoryRepository.GetInventoryItemsByNameAsync(partName);
        }

        public async Task<Inventory> CreateInventoryItemAsync(
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            return await _inventoryRepository.CreateInventoryItemAsync(
                partName, quantity, unitPrice, expiryDate, warrantyMonths);
        }

        public async Task<Inventory> UpdateInventoryItemAsync(
            int partId,
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            return await _inventoryRepository.UpdateInventoryItemAsync(
                partId, partName, quantity, unitPrice, expiryDate, warrantyMonths);
        }

        public async Task<bool> RemoveInventoryItemAsync(int partId)
        {
            return await _inventoryRepository.RemoveInventoryItemAsync(partId);
        }

        //This is revenue report
        public async Task<RevenueReportDto> GenerateRevenueReport(DateTime startDate, DateTime endDate)
        {
            // Validate date range
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be before end date");
            }
            return await _inventoryRepository.GenerateRevenueReport(startDate, endDate);
        }
    }
}
