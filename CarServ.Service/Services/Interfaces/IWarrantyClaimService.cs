using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface IWarrantyClaimService
    {
        Task<WarrantyClaim> GetWarrantyClaimByIdAsync(int claimId);
        Task<List<WarrantyClaim>> GetWarrantyClaimsBySupplierIdAsync(int supplierId);
        Task<List<WarrantyClaim>> GetAllWarrantyClaimsAsync();
        Task<List<WarrantyClaim>> GetWarrantyClaimsByStatusAsync(string status);
        Task<List<WarrantyClaim>> GetWarrantyClaimsByClaimDateAsync(DateOnly claimDate);
        Task<List<WarrantyClaim>> GetWarrantyClaimsByNoteAsync(string note);
        Task<WarrantyClaim> CreateWarrantyClaimAsync(
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes);
        Task<WarrantyClaim> UpdateWarrantyClaimAsync(
            int claimId,
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes);
        Task<WarrantyClaim> DeactivateWarrantyClaimAsync(int claimId);
    }
}
