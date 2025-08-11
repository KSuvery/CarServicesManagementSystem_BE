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
    public class WarrantyClaimService : IWarrantyClaimService
    {
        private readonly IWarrantyClaimRepository _warrantyClaimRepository;
        public WarrantyClaimService(IWarrantyClaimRepository warrantyClaimRepository)
        {
            _warrantyClaimRepository = warrantyClaimRepository;
        }

        public async Task<WarrantyClaim> GetWarrantyClaimByIdAsync(int claimId)
        {
            return await _warrantyClaimRepository.GetWarrantyClaimByIdAsync(claimId);
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimsBySupplierIdAsync(int supplierId)
        {
            return await _warrantyClaimRepository.GetWarrantyClaimsBySupplierIdAsync(supplierId);
        }

        public async Task<List<WarrantyClaim>> GetAllWarrantyClaimsAsync()
        {
            return await _warrantyClaimRepository.GetAllWarrantyClaimsAsync();
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimsByStatusAsync(string status)
        {
            return await _warrantyClaimRepository.GetWarrantyClaimsByStatusAsync(status);
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimsByClaimDateAsync(DateOnly claimDate)
        {
            return await _warrantyClaimRepository.GetWarrantyClaimsByClaimDateAsync(claimDate);
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimsByNoteAsync(string note)
        {
            return await _warrantyClaimRepository.GetWarrantyClaimsByNoteAsync(note);
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
