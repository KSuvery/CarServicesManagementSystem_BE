using CarServ.Domain.Entities;
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

        public async Task<List<Parts>> GetAllPartsAsync()
        {
            return await _partsRepository.GetAllPartsAsync();
        }

        public async Task<Parts> GetPartByIdAsync(int partId)
        {
            return await _partsRepository.GetPartByIdAsync(partId);
        }

        public async Task<List<Parts>> GetPartsByPartName(string partName)
        {
            return await _partsRepository.GetPartsByPartName(partName);
        }

        public async Task<List<Parts>> GetPartsByUnitPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _partsRepository.GetPartsByUnitPriceRange(minPrice, maxPrice);
        }

        public async Task<List<Parts>> GetPartsByExpiryDateRange(DateOnly startDate, DateOnly endDate)
        {
            return await _partsRepository.GetPartsByExpiryDateRange(startDate, endDate);
        }

        public async Task<List<Parts>> GetPartsByWarrantyMonthsRange(int minMonths, int maxMonths)
        {
            return await _partsRepository.GetPartsByWarrantyMonthsRange(minMonths, maxMonths);
        }

        public async Task<Parts> AddPartAsync(
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

        public async Task<Parts> UpdatePartAsync(
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
    }
}
