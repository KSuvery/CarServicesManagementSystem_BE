using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Logging_part_usage;
using CarServ.Repository.Repositories.DTO.RevenueReport;
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

        public async Task<RevenueReportDto> GenerateRevenueReport(DateTime startDate, DateTime endDate)
        {
            var report = new RevenueReportDto
            {
                StartDate = startDate,
                EndDate = endDate
            };

            // Calculate total payments in date range
            report.TotalRevenue = (decimal)await _context.Payments
                .Where(p => p.PaidAt >= startDate && p.PaidAt <= endDate)
                .SumAsync(p => p.Amount);

            // Calculate service revenue (from packages)
            report.ServiceRevenue = (decimal)await _context.Payments
                .Where(p => p.PaidAt >= startDate && p.PaidAt <= endDate)
                .Join(_context.Appointments,
                    payment => payment.AppointmentId,
                    appointment => appointment.AppointmentId,
                    (payment, appointment) => appointment.PackageId)
                .Join(_context.ServicePackages,
                    packageId => packageId,
                    package => package.PackageId,
                    (packageId, package) => package.Price)
                .SumAsync();

            // Calculate parts revenue
            report.PartsRevenue = (decimal)await _context.PartsUsed
                .Where(pu => _context.ServiceHistory
                    .Any(sh => sh.ServiceId == pu.ServiceId &&
                              sh.ServiceDate >= startDate &&
                              sh.ServiceDate <= endDate))
                .Join(_context.Inventory,
                    part => part.PartId,
                    inventory => inventory.PartId,
                    (part, inventory) => part.QuantityUsed * inventory.UnitPrice)
                .SumAsync();

            // Revenue by service package
            report.RevenueByPackages = await _context.ServicePackages
                .Select(sp => new RevenueByPackage
                {
                    PackageId = sp.PackageId,
                    PackageName = sp.Name,
                    ServiceCount = _context.Appointments
                        .Count(a => a.PackageId == sp.PackageId &&
                                   a.AppointmentDate >= startDate &&
                                   a.AppointmentDate <= endDate),
                    Revenue = (decimal)_context.Appointments
                        .Where(a => a.PackageId == sp.PackageId &&
                                  a.AppointmentDate >= startDate &&
                                  a.AppointmentDate <= endDate)
                        .Join(_context.Payments,
                            appointment => appointment.AppointmentId,
                            payment => payment.AppointmentId,
                            (appointment, payment) => payment.Amount)
                        .Sum()
                })
                .Where(r => r.ServiceCount > 0)
                .ToListAsync();

            // Revenue by vehicle type
            report.RevenueByVehicleTypes = await _context.Vehicles
                .GroupBy(v => v.Type)
                .Select(g => new RevenueByVehicleType
                {
                    VehicleType = g.Key,
                    VehicleCount = g.Count(),
                    Revenue = (decimal)g.Join(_context.Appointments,
                            vehicle => vehicle.VehicleId,
                            appointment => appointment.VehicleId,
                            (vehicle, appointment) => appointment)
                        .Where(a => a.AppointmentDate >= startDate &&
                                  a.AppointmentDate <= endDate)
                        .Join(_context.Payments,
                            appointment => appointment.AppointmentId,
                            payment => payment.AppointmentId,
                            (appointment, payment) => payment.Amount)
                        .Sum()
                })
                .Where(r => r.Revenue > 0)
                .ToListAsync();

            return report;
        }


        public void TrackPartsUsed(PartUsageDto partsUsedDTO)
        {
            // Create a new PartsUsed entity
            var partsUsed = new PartsUsed
            {
                ServiceId = partsUsedDTO.ServiceID,
                PartId = partsUsedDTO.PartID,
                QuantityUsed = partsUsedDTO.QuantityUsed
            };
            // Add to the PartsUsed table
            _context.PartsUsed.Add(partsUsed);
            // Update the inventory stock level
            var inventoryItem = _context.Inventory.Find(partsUsedDTO.PartID);
            if (inventoryItem != null)
            {
                inventoryItem.Quantity -= partsUsedDTO.QuantityUsed;
                _context.Inventory.Update(inventoryItem);
            }
            // Save changes to the database
            _context.SaveChanges();
        }

    }
}
