using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services.Interfaces
{
    public interface IWarrantyClaimervice
    {
        Task<WarrantyClaim> GetWarrantyClaimByIdAsync(int claimId);
        Task<List<WarrantyClaim>> GetWarrantyClaimBySupplierIdAsync(int supplierId);
        Task<List<WarrantyClaim>> GetAllWarrantyClaimAsync();
        Task<List<WarrantyClaim>> GetWarrantyClaimByStatusAsync(string status);
        Task<List<WarrantyClaim>> GetWarrantyClaimByClaimDateAsync(DateOnly claimDate);
        Task<List<WarrantyClaim>> GetWarrantyClaimByNoteAsync(string note);
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
