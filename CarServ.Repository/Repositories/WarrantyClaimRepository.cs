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
    public class WarrantyClaimRepository : GenericRepository<WarrantyClaim>, IWarrantyClaimRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public WarrantyClaimRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<WarrantyClaim> GetWarrantyClaimByIdAsync(int claimId)
        {
            return await _context.WarrantyClaims
                .FirstOrDefaultAsync(c => c.ClaimId == claimId);
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimBySupplierIdAsync(int supplierId)
        {
            return await _context.WarrantyClaims
                .Where(c => c.SupplierId == supplierId)
                .ToListAsync();
        }

        public async Task<List<WarrantyClaim>> GetAllWarrantyClaimAsync()
        {
            return await _context.WarrantyClaims.ToListAsync();
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimByStatusAsync(string status)
        {
            return await _context.WarrantyClaims
                .Where(c => c.Status != null &&
                    c.Status.ToLower().Contains(status.ToLower()))
                .ToListAsync();
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimByClaimDateAsync(DateOnly claimDate)
        {
            return await _context.WarrantyClaims
                .Where(c => c.ClaimDate == claimDate)
                .ToListAsync();
        }

        public async Task<List<WarrantyClaim>> GetWarrantyClaimByNoteAsync(string note)
        {
            return await _context.WarrantyClaims
                .Where(c => c.Notes != null &&
                    c.Notes.ToLower().Contains(note.ToLower()))
                .ToListAsync();
        }

        public async Task<WarrantyClaim> CreateWarrantyClaimAsync(
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes
            )
        {
            status = "Pending";
            var warrantyClaim = new WarrantyClaim
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

        public async Task<WarrantyClaim> UpdateWarrantyClaimAsync(
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

        public async Task<WarrantyClaim> DeactivateWarrantyClaimAsync(int claimId)
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
