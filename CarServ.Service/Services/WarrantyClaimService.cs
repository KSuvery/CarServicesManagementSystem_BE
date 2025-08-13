using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services
{
    public class WarrantyClaimervice : IWarrantyClaimervice
    {
        private readonly IWarrantyClaimRepository _warrantyClaimRepository;
        public WarrantyClaimervice(IWarrantyClaimRepository warrantyClaimRepository)
        {
            _warrantyClaimRepository = warrantyClaimRepository;
        }

        public async Task<WarrantyClaim> GetWarrantyClaimByIdAsync(int claimId)
        {
            return await _warrantyClaimRepository.GetWarrantyClaimByIdAsync(claimId);
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimBySupplierIdAsync(int supplierId)
        {
            return await _warrantyClaimRepository.GetWarrantyClaimBySupplierIdAsync(supplierId);
        }

        public async Task<List<WarrantyClaim>> GetAllWarrantyClaimAsync()
        {
            return await _warrantyClaimRepository.GetAllWarrantyClaimAsync();
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimByStatusAsync(string status)
        {
            return await _warrantyClaimRepository.GetWarrantyClaimByStatusAsync(status);
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimByClaimDateAsync(DateOnly claimDate)
        {
            return await _warrantyClaimRepository.GetWarrantyClaimByClaimDateAsync(claimDate);
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimByNoteAsync(string note)
        {
            return await _warrantyClaimRepository.GetWarrantyClaimByNoteAsync(note);
        }

        public async Task<WarrantyClaim> CreateWarrantyClaimAsync(
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes)
        {
            return await _warrantyClaimRepository.CreateWarrantyClaimAsync(partId, supplierId, claimDate, status, notes);
        }

        public async Task<WarrantyClaim> UpdateWarrantyClaimAsync(
            int claimId,
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes)
        {
            return await _warrantyClaimRepository.UpdateWarrantyClaimAsync(claimId, partId, supplierId, claimDate, status, notes);
        }

        public async Task<WarrantyClaim> DeactivateWarrantyClaimAsync(int claimId)
        {
            return await _warrantyClaimRepository.DeactivateWarrantyClaimAsync(claimId);
        }
    }
}
