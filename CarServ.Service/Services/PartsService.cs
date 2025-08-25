using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
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

        public async Task<List<PartDto>> GetAllPartsAsync()
        {
            return await _partsRepository.GetAllPartsAsync();
        }

        public async Task<PartDto> GetPartByIdAsync(int partId)
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
        public async Task<RevenueReportDto> GenerateRevenueReport(int month, int year)
        {
            return await _partsRepository.GenerateRevenueReport(month, year);
        }

        public async Task TrackPartsUsed(PartUsageDto partUsage)
        {
            await _partsRepository.TrackPartsUsed(partUsage);
        }

        public async Task UpdateServiceProgress(UpdateServiceProgressDto dto)
        {
            await _partsRepository.UpdateServiceProgress(dto);
        }

        public async Task<List<PartDto>> GetLowPartsAsync()
        {
            return await _partsRepository.GetLowPartsAsync();
        }

        public async Task<List<Part>> GetZeroPartsAsync()
        {
            return await _partsRepository.GetZeroPartsAsync();
        }

        public async Task<PaginationResult<List<PartDto>>> GetAllPartsWithPaging(int currentPage, int pageSize)
        {
            return await _partsRepository.GetAllPartsWithPaging(currentPage, pageSize);
        }

        public async Task<PaginationResult<List<Supplier>>> GetAllSuppliersAsync(int currentPage, int pageSize)
        {
            return await _partsRepository.GetAllSuppliersAsync(currentPage, pageSize);
        }

        public async Task<Part> CreatePartAsync(CreatePartDto dto)
        {
            return await _partsRepository.CreatePartAsync(dto);
        }

        public async Task<Part> UpdatePartAsync(int partId, UpdatePartDto dto)
        {
            return await _partsRepository.UpdatePartAsync(partId, dto);
        }

        public async Task DeletePartAsync(int partId)
        {
            await _partsRepository.DeletePartAsync(partId);  
        }

        public async Task<DashboardSummaryDto> GenerateDashboardSummary(int month, int year)
        {
            return await _partsRepository.GenerateDashboardSummary(month, year);
        }
    }
}
