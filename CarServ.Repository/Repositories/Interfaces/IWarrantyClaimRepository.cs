using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IWarrantyClaimRepository : IGenericRepository<WarrantyClaims>
    {
        Task<WarrantyClaims> GetWarrantyClaimByIdAsync(int claimId);
        Task<List<WarrantyClaims>> GetWarrantyClaimsBySupplierIdAsync(int supplierId);
        Task<List<WarrantyClaims>> GetAllWarrantyClaimsAsync();
        Task<List<WarrantyClaims>> GetWarrantyClaimsByStatusAsync(string status);
        Task<List<WarrantyClaims>> GetWarrantyClaimsByClaimDateAsync(DateOnly claimDate);
        Task<List<WarrantyClaims>> GetWarrantyClaimsByNoteAsync(string note);
        Task<WarrantyClaims> CreateWarrantyClaimAsync(
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes);
        Task<WarrantyClaims> UpdateWarrantyClaimAsync(
            int claimId,
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes);
        Task<WarrantyClaims> DeactivateWarrantyClaimAsync(int claimId);
    }
}
