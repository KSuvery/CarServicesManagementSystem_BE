using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories
{
    public class PartsRepository : GenericRepository<Parts>, IPartsRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public PartsRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<List<Parts>> GetAllPartsAsync()
        {
            return await _context.Parts.ToListAsync();
        }

        public async Task<Parts> GetPartByIdAsync(int partId)
        {
            return await _context.Parts.FindAsync(partId);
        }

        public async Task<List<Parts>> GetPartsByPartName(string partName)
        {
            var parts = await _context.Parts
                .Where(p => p.PartName.ToLower().Contains(partName.ToLower()))
                .ToListAsync();

            return parts;
        }

        public async Task<List<Parts>> GetPartsByUnitPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _context.Parts
                .Where(p => p.UnitPrice >= minPrice && p.UnitPrice <= maxPrice)
                .ToListAsync();
        }

        public async Task<List<Parts>> GetPartsByExpiryDateRange(DateOnly startDate, DateOnly endDate)
        {
            return await _context.Parts
                .Where(p => p.ExpiryDate >= startDate && p.ExpiryDate <= endDate)
                .ToListAsync();
        }

        public async Task<List<Parts>> GetPartsByWarrantyMonthsRange(int minMonths, int maxMonths)
        {
            return await _context.Parts
                .Where(p => p.WarrantyMonths >= minMonths && p.WarrantyMonths <= maxMonths)
                .ToListAsync();
        }

        public async Task<Parts> AddPartAsync(
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths)
        {
            var newPart = new Parts
            {
                PartName = partName,
                Quantity = quantity,
                UnitPrice = unitPrice,
                ExpiryDate = expiryDate,
                WarrantyMonths = warrantyMonths
            };
            await _context.Parts.AddAsync(newPart);
            await _context.SaveChangesAsync();
            return newPart;
        }

        public async Task<Parts> UpdatePartAsync(
            int partId,
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths)
        {
             var part = await GetPartByIdAsync(partId);
            if (part == null)
            {
                return null; // or throw an exception
            }
            part.PartName = partName;
            part.Quantity = quantity;
            part.UnitPrice = unitPrice;
            part.ExpiryDate = expiryDate;
            part.WarrantyMonths = warrantyMonths;
            _context.Parts.Update(part);
            await _context.SaveChangesAsync();
            return part;
        }
    }
}
