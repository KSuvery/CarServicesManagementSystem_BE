using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Logging_part_usage;
using CarServ.Repository.Repositories.DTO.RevenueReport;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories
{
    public class PartRepository : GenericRepository<Part>, IPartRepository
    {
        private readonly CarServicesManagementSystemContext _context;        


        public PartRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
            
        }

        public async Task<List<Part>> GetAllPartItemsAsync()
        {
            return await _context.Parts.ToListAsync();
        }

        public async Task<Part> GetPartItemByIdAsync(int partId)
        {
            return await _context.Parts
                .FirstOrDefaultAsync(i => i.PartId == partId);
        }

        public async Task<List<Part>> GetPartItemsByNameAsync(string partName)
        {
            return await _context.Parts
                .Where(i => i.PartName.Contains(partName))
                .ToListAsync();
        }

        public async Task<Part> CreatePartItemAsync(
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            var PartItem = new Part
            {
                PartName = partName,
                Quantity = quantity,
                UnitPrice = unitPrice,
                ExpiryDate = expiryDate,
                WarrantyMonths = warrantyMonths
            };
            _context.Parts.Add(PartItem);
            await _context.SaveChangesAsync();
            return PartItem;
        }

        public async Task<Part> UpdatePartItemAsync(
            int partId,
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            var PartItem = await GetPartItemByIdAsync(partId);
            if (PartItem == null)
            {
                return null; // or throw an exception
            }
            PartItem.PartName = partName;
            PartItem.Quantity = quantity;
            PartItem.UnitPrice = unitPrice;
            PartItem.ExpiryDate = expiryDate;
            PartItem.WarrantyMonths = warrantyMonths;
            _context.Parts.Update(PartItem);
            await _context.SaveChangesAsync();
            return PartItem;
        }

        public async Task<bool> RemovePartItemAsync(int partId)
        {
            var PartItem = await GetPartItemByIdAsync(partId);
            if (PartItem == null)
            {
                return false; // or throw an exception
            }
            _context.Parts.Remove(PartItem);
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

            // Calculate total Payment in date range
            report.TotalRevenue = (decimal)await _context.Payment
                .Where(p => p.PaidAt >= startDate && p.PaidAt <= endDate)
                .SumAsync(p => p.Amount);

            // Calculate service revenue (from packages)
            report.ServiceRevenue = (decimal)await _context.Payment
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
            /*report.PartsRevenue = (decimal)await _context.PartPrices
                .Where(pu => _context.ServiceHistory
                    .Any(sh => sh.ServiceId == pu.ServiceId &&
                              sh.ServiceDate >= startDate &&
                              sh.ServiceDate <= endDate))
                .Join(_context.Part,
                    part => part.PartId,
                    Part => Part.PartId,
                    (part, Part) => part.QuantityUsed * Part.UnitPrice)
                .SumAsync();*/

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
                        .Join(_context.Payment,
                            appointment => appointment.AppointmentId,
                            payment => payment.AppointmentId,
                            (appointment, payment) => payment.Amount)
                        .Sum()
                })
                .Where(r => r.ServiceCount > 0)
                .ToListAsync();

            // Revenue by vehicle type
            report.RevenueByVehicleTypes = await _context.Vehicles
                .GroupBy(v => v.CarType)
                .Select(g => new RevenueByVehicleType
                {
                    VehicleType = g.Key.TypeName,
                    VehicleCount = g.Count(),
                    Revenue = (decimal)g.Join(_context.Appointments,
                            vehicle => vehicle.VehicleId,
                            appointment => appointment.VehicleId,
                            (vehicle, appointment) => appointment)
                        .Where(a => a.AppointmentDate >= startDate &&
                                  a.AppointmentDate <= endDate)
                        .Join(_context.Payment,
                            appointment => appointment.AppointmentId,
                            payment => payment.AppointmentId,
                            (appointment, payment) => payment.Amount)
                        .Sum()
                })
                .Where(r => r.Revenue > 0)
                .ToListAsync();

            return report;
        }


        public async Task TrackPartsUsed(PartUsageDto partsUsedDTO)
        {
            /*var partsUsed = new PartsUsed
            {
                ServiceId = partsUsedDTO.ServiceID,
                PartId = partsUsedDTO.PartID,
                QuantityUsed = partsUsedDTO.QuantityUsed
            };

            _context.PartsUsed.Add(partsUsed);

            // Update the Part stock level
            var PartItem = _context.Part.Find(partsUsedDTO.PartID);
            if (PartItem != null)
            {
                PartItem.Quantity -= partsUsedDTO.QuantityUsed;
                _context.Part.Update(PartItem);
                await _context.SaveChangesAsync();
                
            }

            await CheckAndNotifyLowStock(PartItem);*/
        }

        private async Task CheckAndNotifyLowStock(Part item)
        {
           /* if (item != null && item.Quantity < 3)
            {
                var message = $"Low stock alert: {item.PartName} (ID: {item.PartId}) " +
                              $"has only {item.Quantity} units remaining!";

                var managers = await _context.PartManagers
                    .Include(m => m.Manager)
                    .ToListAsync();

                var Notification = new List<Notification>();

                foreach (var manager in managers)
                {
                    Notification.Add(new Notification
                    {
                        UserId = manager.ManagerId,
                        Message = $"Low Stock Alert: {message}",
                        SentAt = DateTime.Now,
                        IsRead = false
                    });
                }

                _context.Notification.AddRange(Notification);
                await _context.SaveChangesAsync(); 
            }*/
        }


    }
}
