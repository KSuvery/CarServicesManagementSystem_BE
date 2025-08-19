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
    public class PartsService : IPartsService
    {
        private readonly IPartsRepository _partsRepository;
        public PartsService(IPartsRepository partsRepository)
        {
            _partsRepository = partsRepository;
        }

        public async Task<List<Part>> GetAllPartsAsync()
        {
            return await _partsRepository.GetAllPartsAsync();
        }

        public async Task<Part> GetPartByIdAsync(int partId)
        {
            return await _partsRepository.GetPartByIdAsync(partId);
        }

        public async Task<List<Part>> GetPartsByPartName(string partName)
        {
            return await _partsRepository.GetPartsByPartName(partName);
        }

        public async Task<List<Part>> GetPartsByUnitPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _partsRepository.GetPartsByUnitPriceRange(minPrice, maxPrice);
        }

        public async Task<List<Part>> GetPartsByExpiryDateRange(DateOnly startDate, DateOnly endDate)
        {
            return await _partsRepository.GetPartsByExpiryDateRange(startDate, endDate);
        }

        public async Task<List<Part>> GetPartsByWarrantyMonthsRange(int minMonths, int maxMonths)
        {
            return await _partsRepository.GetPartsByWarrantyMonthsRange(minMonths, maxMonths);
        }

        public async Task<Part> AddPartAsync(
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths)
        {
            return await _partsRepository.AddPartAsync(
                partName,
                quantity,
                unitPrice,
                expiryDate,
                warrantyMonths);
        }

        public async Task<Part> UpdatePartAsync(
            int partId,
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths)
        {
            return await _partsRepository.UpdatePartAsync(
                partId,
                partName,
                quantity,
                unitPrice,
                expiryDate,
                warrantyMonths);
        }

        //Nhat's Methods
        public async Task<RevenueReportDto> GenerateRevenueReport(DateTime startDate, DateTime endDate)
        {
            return await _partsRepository.GenerateRevenueReport(startDate, endDate);
        }

        public async Task TrackPartsUsed(PartUsageDto partUsage)
        {
            await _partsRepository.TrackPartsUsed(partUsage);
        }

        public async Task UpdateServiceProgress(UpdateServiceProgressDto dto)
        {
            await _partsRepository.UpdateServiceProgress(dto);
        }

        public async Task<List<Part>> GetLowPartsAsync()
        {
            return await _partsRepository.GetLowPartsAsync();
        }

        public async Task<List<Part>> GetZeroPartsAsync()
        {
            return await _partsRepository.GetZeroPartsAsync();
        }
    }
}
