using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Logging_part_usage;
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
    public class PartServices : IPartServices
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public PartServices(IInventoryRepository inventoryRepository, IAppointmentRepository appointmentRepository)
        {
            _inventoryRepository = inventoryRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<Part>> GetAllPartItemsAsync()
        {
            return await _inventoryRepository.GetAllInventoryItemsAsync();
        }

        public async Task<Part> GetPartItemByIdAsync(int partId)
        {
            return await _inventoryRepository.GetInventoryItemByIdAsync(partId);
        }

        public async Task<List<Part>> GetPartItemsByNameAsync(string partName)
        {
            return await _inventoryRepository.GetInventoryItemsByNameAsync(partName);
        }

        public async Task<Part> CreatePartItemAsync(
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            return await _inventoryRepository.CreateInventoryItemAsync(
                partName, quantity, unitPrice, expiryDate, warrantyMonths);
        }

        public async Task<Part> UpdatePartItemAsync(
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

        public async Task<bool> RemovePartItemAsync(int partId)
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

        public void TrackPartsUsed(PartUsageDto partsUsedDTO)
        {
            // You can add any additional business logic here if needed
            _inventoryRepository.TrackPartsUsed(partsUsedDTO);
        }

        public async Task UpdateServiceProgress(UpdateServiceProgressDto dto)
        {
            await _inventoryRepository.UpdateServiceProgress(dto);
        }
    }
}
