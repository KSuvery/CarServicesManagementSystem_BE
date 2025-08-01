using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories
{
    public class WarrantyClaimRepository : GenericRepository<WarrantyClaims>, IWarrantyClaimRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public WarrantyClaimRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<WarrantyClaims> GetWarrantyClaimByIdAsync(int claimId)
        {
            return await _context.WarrantyClaims
                .FirstOrDefaultAsync(c => c.ClaimId == claimId);
        }

        public async Task<List<WarrantyClaims>> GetWarrantyClaimsBySupplierIdAsync(int supplierId)
        {
            return await _context.WarrantyClaims
                .Where(c => c.SupplierId == supplierId)
                .ToListAsync();
        }

        public async Task<List<WarrantyClaims>> GetAllWarrantyClaimsAsync()
        {
            return await _context.WarrantyClaims.ToListAsync();
        }

        public async Task<List<WarrantyClaims>> GetWarrantyClaimsByStatusAsync(string status)
        {
            return await _context.WarrantyClaims
                .Where(c => c.Status != null &&
                    c.Status.ToLower().Contains(status.ToLower()))
                .ToListAsync();
        }

        public async Task<List<WarrantyClaims>> GetWarrantyClaimsByClaimDateAsync(DateOnly claimDate)
        {
            return await _context.WarrantyClaims
                .Where(c => c.ClaimDate == claimDate)
                .ToListAsync();
        }

        public async Task<List<WarrantyClaims>> GetWarrantyClaimsByNoteAsync(string note)
        {
            return await _context.WarrantyClaims
                .Where(c => c.Notes != null &&
                    c.Notes.ToLower().Contains(note.ToLower()))
                .ToListAsync();
        }

        public async Task<WarrantyClaims> CreateWarrantyClaimAsync(
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes
            )
        {
            status = "Pending";
            var warrantyClaim = new WarrantyClaims
            {
                PartId = partId,
                SupplierId = supplierId,
                ClaimDate = claimDate,
                Status = status,
                Notes = notes
            };
            _context.WarrantyClaims.Add(warrantyClaim);
            await _context.SaveChangesAsync();
            return warrantyClaim;
        }

        public async Task<WarrantyClaims> UpdateWarrantyClaimAsync(
            int claimId,
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes
            )
        {
            var warrantyClaim = await GetWarrantyClaimByIdAsync(claimId);
            if (warrantyClaim == null)
                return null;
            warrantyClaim.PartId = partId;
            warrantyClaim.SupplierId = supplierId;
            warrantyClaim.ClaimDate = claimDate;
            warrantyClaim.Status = status;
            warrantyClaim.Notes = notes;
            _context.WarrantyClaims.Update(warrantyClaim);
            await _context.SaveChangesAsync();
            return warrantyClaim;
        }

        public async Task<WarrantyClaims> DeactivateWarrantyClaimAsync(int claimId)
        {
            var warrantyClaim = await GetWarrantyClaimByIdAsync(claimId);
            if (warrantyClaim == null)
                return null;
            warrantyClaim.Status = "Deactivated";
            _context.WarrantyClaims.Update(warrantyClaim);
            await _context.SaveChangesAsync();
            return warrantyClaim;
        }
    }
}
