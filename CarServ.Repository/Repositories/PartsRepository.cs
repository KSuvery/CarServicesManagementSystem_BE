using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Logging_part_usage;
using CarServ.Repository.Repositories.DTO.RevenueReport;
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
    public class PartsRepository : GenericRepository<Part>, IPartsRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public PartsRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }
        
        public async Task<List<Part>> GetAllPartsAsync()
        {
            return await _context.Parts.ToListAsync();
        }
        public async Task<List<Part>> GetLowPartsAsync()
        {
            var parts = await _context.Parts
                .Where(p => p.Quantity < 5)
                .ToListAsync();

            return parts;
        }
        public async Task<List<Part>> GetZeroPartsAsync()
        {
            var parts = await _context.Parts
                .Where(p => p.Quantity <= 0)
                .ToListAsync();

            return parts;
        }

        public async Task<Part> GetPartByIdAsync(int partId)
        {
            return await _context.Parts.FindAsync(partId);
        }

        public async Task<List<Part>> GetPartsByPartName(string partName)
        {
            var parts = await _context.Parts
                .Where(p => p.PartName.ToLower().Contains(partName.ToLower()))
                .ToListAsync();

            return parts;
        }

        public async Task<List<Part>> GetPartsByUnitPriceRange(decimal minPrice, decimal maxPrice)
        {
            return await _context.Parts
                .Where(p => p.UnitPrice >= minPrice && p.UnitPrice <= maxPrice)
                .ToListAsync();
        }

        public async Task<List<Part>> GetPartsByExpiryDateRange(DateOnly startDate, DateOnly endDate)
        {
            return await _context.Parts
                .Where(p => p.ExpiryDate >= startDate && p.ExpiryDate <= endDate)
                .ToListAsync();
        }

        public async Task<List<Part>> GetPartsByWarrantyMonthsRange(int minMonths, int maxMonths)
        {
            return await _context.Parts
                .Where(p => p.WarrantyMonths >= minMonths && p.WarrantyMonths <= maxMonths)
                .ToListAsync();
        }

        public async Task<Part> AddPartAsync(
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths)
        {
            var newPart = new Part
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

        public async Task<Part> UpdatePartAsync(
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

        public async Task<RevenueReportDto> GenerateRevenueReport(DateTime startDate, DateTime endDate)
        {
            // Validate the input dates
            if (startDate > endDate)
            {
                throw new ArgumentException("Start date must be earlier than end date.");
            }


            var orders = await _context.Orders
                .Include(o => o.Payments)
                .Include(o => o.Appointment)
                .ThenInclude(op => op.AppointmentServices)
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .ToListAsync();

            var totalRevenue = orders.Sum(o => o.Payments.Sum(p => p.Amount) ?? 0);
            var totalOrders = orders.Count;

            return new RevenueReportDto
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue
            };
        }

        public async Task UpdateServiceProgress(UpdateServiceProgressDto dto)
        {
          
            if (string.IsNullOrEmpty(dto.Status) || !IsValidStatus(dto.Status))
            {
                throw new ArgumentException("Invalid status provided.");
            }

            
            var serviceProgress = await _context.ServiceProgresses
                .FirstOrDefaultAsync(sp => sp.AppointmentId == dto.AppointmentId);

            if (serviceProgress == null)
            {
                throw new InvalidOperationException("Service progress not found for the given appointment.");
            }

            
            serviceProgress.Status = dto.Status;
            serviceProgress.Note = dto.Note;
            serviceProgress.UpdatedAt = DateTime.Now;

            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == dto.AppointmentId);
            
            if (dto.Status == "Completed")
            {
                if (appointment != null)
                {
                    var vehicle = await _context.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == appointment.VehicleId);
                    vehicle.Status = "Available";
                    _context.Vehicles.Update(vehicle);
                    await _context.SaveChangesAsync();

                }
                await ReduceUsedParts(dto.AppointmentId);
                await CheckLowStockAndNotify();
            }
           
            await _context.SaveChangesAsync();
        }
        private async Task CheckLowStockAndNotify()
        {
            
            var lowStockParts = await _context.Parts
                .Where(p => p.Quantity < 2)
                .ToListAsync();
            
            var userIdsToNotify = await _context.Users
                .Where(u => u.RoleId == 1 || u.RoleId == 4)
                .Select(u => u.UserId)
                .ToListAsync();
            foreach (var part in lowStockParts)
            {
                foreach (var userId in userIdsToNotify)
                {
                    var notification = new Notification
                    {
                        UserId = userId,
                        Message = $"Low stock alert: The quantity of part '{part.PartName}' is low (Current Quantity: {part.Quantity}).",
                        SentAt = DateTime.Now,
                        IsRead = false
                    };
                    await _context.Notifications.AddAsync(notification);
                }
            }
        }
            private bool IsValidStatus(string status)
        {
            var validStatuses = new[] { "Booked", "Vehicle Received", "In Service", "Completed", "Cancelled" };
            return validStatuses.Contains(status);
        }

        private async Task ReduceUsedParts(int appointmentId)
        {
            // Get the parts used in the appointment
            var appointmentServices = await _context.AppointmentServices
                .Include(a => a.Service)
                .Where(a => a.AppointmentId == appointmentId)
                .ToListAsync();

            foreach (var appointmentService in appointmentServices)
            {
                var serviceParts = await _context.ServiceParts
                    .Where(sp => sp.ServiceId == appointmentService.ServiceId)
                    .ToListAsync();

                foreach (var servicePart in serviceParts)
                {
                    var part = await _context.Parts.FindAsync(servicePart.PartId);
                    if (part != null && part.Quantity.HasValue)
                    {

                        part.Quantity -= servicePart.QuantityRequired;
                        if (part.Quantity < 0)
                        {
                            part.Quantity = 0;
                        }
                    }
                }
            }
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
    }
}
