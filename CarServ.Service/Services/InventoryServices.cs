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
        private readonly IPartRepository _PartRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public PartServices(IPartRepository PartRepository, IAppointmentRepository appointmentRepository)
        {
            _PartRepository = PartRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<Part>> GetAllPartItemsAsync()
        {
            return await _PartRepository.GetAllPartItemsAsync();
        }

        public async Task<Part> GetPartItemByIdAsync(int partId)
        {
            return await _PartRepository.GetPartItemByIdAsync(partId);
        }

        public async Task<List<Part>> GetPartItemsByNameAsync(string partName)
        {
            return await _PartRepository.GetPartItemsByNameAsync(partName);
        }

        public async Task<Part> CreatePartItemAsync(
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            return await _PartRepository.CreatePartItemAsync(
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
            return await _PartRepository.UpdatePartItemAsync(
                partId, partName, quantity, unitPrice, expiryDate, warrantyMonths);
        }

        public async Task<bool> RemovePartItemAsync(int partId)
        {
            return await _PartRepository.RemovePartItemAsync(partId);
        }

        //This is revenue report
        public async Task<RevenueReportDto> GenerateRevenueReport(DateTime startDate, DateTime endDate)
        {
            // Validate date range
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be before end date");
            }
            return await _PartRepository.GenerateRevenueReport(startDate, endDate);
        }

        public void TrackPartsUsed(PartUsageDto partsUsedDTO)
        {
            // You can add any additional business logic here if needed
            _PartRepository.TrackPartsUsed(partsUsedDTO);
        }
    }
}
