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
    public class InventoryRepository : GenericRepository<Inventory>, IInventoryRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public InventoryRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Inventory>> GetAllInventoryItemsAsync()
        {
            return await _context.Inventory.ToListAsync();
        }

        public async Task<Inventory> GetInventoryItemByIdAsync(int partId)
        {
            return await _context.Inventory
                .FirstOrDefaultAsync(i => i.PartId == partId);
        }

        public async Task<List<Inventory>> GetInventoryItemsByNameAsync(string partName)
        {
            return await _context.Inventory
                .Where(i => i.PartName.Contains(partName))
                .ToListAsync();
        }

        public async Task<Inventory> CreateInventoryItemAsync(
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            var inventoryItem = new Inventory
            {
                PartName = partName,
                Quantity = quantity,
                UnitPrice = unitPrice,
                ExpiryDate = expiryDate,
                WarrantyMonths = warrantyMonths
            };
            _context.Inventory.Add(inventoryItem);
            await _context.SaveChangesAsync();
            return inventoryItem;
        }

        public async Task<Inventory> UpdateInventoryItemAsync(
            int partId,
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            var inventoryItem = await GetInventoryItemByIdAsync(partId);
            if (inventoryItem == null)
            {
                return null; // or throw an exception
            }
            inventoryItem.PartName = partName;
            inventoryItem.Quantity = quantity;
            inventoryItem.UnitPrice = unitPrice;
            inventoryItem.ExpiryDate = expiryDate;
            inventoryItem.WarrantyMonths = warrantyMonths;
            _context.Inventory.Update(inventoryItem);
            await _context.SaveChangesAsync();
            return inventoryItem;
        }

        public async Task<bool> RemoveInventoryItemAsync(int partId)
        {
            var inventoryItem = await GetInventoryItemByIdAsync(partId);
            if (inventoryItem == null)
            {
                return false; // or throw an exception
            }
            _context.Inventory.Remove(inventoryItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
