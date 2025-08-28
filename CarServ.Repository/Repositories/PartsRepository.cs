using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
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
using static CarServ.Repository.Repositories.PartsRepository;

namespace CarServ.Repository.Repositories
{
    public class PartsRepository : GenericRepository<Part>, IPartsRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public PartsRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }
                
        public async Task<List<PartDto>> GetAllPartsAsync()
        {
            var parts = await _context.Parts
                .Include(p => p.PartPrices) 
                .Include(p => p.WarrantyClaims) 
                    .ThenInclude(w => w.Supplier) 
                .ToListAsync();
            var partDtos = parts.Select(p => new PartDto
            {
                PartId = p.PartId,
                PartName = p.PartName,
                Quantity = p.Quantity,
                Unit = p.Unit,
                CurrentUnitPrice = p.UnitPrice,
                ExpiryDate = p.ExpiryDate,
                WarrantyMonths = p.WarrantyMonths,
                SupplierName = p.WarrantyClaims.FirstOrDefault()?.Supplier?.Name ?? "Someone", 
                PartPrices = p.PartPrices.Select(pp => new PartPriceDto
                {
                    Price = pp.Price,
                    EffectiveFrom = pp.EffectiveFrom
                }).ToList()
            }).ToList();
            return partDtos;
        }

        public async Task<PaginationResult<List<PartDto>>> GetAllPartsWithPaging(int currentPage, int pageSize)
        {
            var userListTmp = await this.GetAllPartsAsync();

            var totalItems = userListTmp.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            userListTmp = userListTmp.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationResult<List<PartDto>>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Items = userListTmp
            };
        }
        public async Task<PaginationResult<List<Supplier>>> GetAllSuppliersAsync(int currentPage, int pageSize)
        {
            var userListTmp = await _context.Suppliers.ToListAsync();            

            var totalItems = userListTmp.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            userListTmp = userListTmp.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationResult<List<Supplier>>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Items = userListTmp
            };
        }

        public async Task<List<PartDto>> GetLowPartsAsync()
        {
            var parts = await _context.Parts
                .Where(p => p.Quantity < 5)
                .Include(p => p.PartPrices)
                .Include(p => p.WarrantyClaims)
                    .ThenInclude(w => w.Supplier)
                .ToListAsync();
            var partDtos = parts.Select(p => new PartDto
            {
                PartId = p.PartId,
                PartName = p.PartName,
                Quantity = p.Quantity,
                Unit = p.Unit,
                CurrentUnitPrice = p.UnitPrice,
                ExpiryDate = p.ExpiryDate,
                WarrantyMonths = p.WarrantyMonths,
                SupplierName = p.WarrantyClaims.FirstOrDefault()?.Supplier?.Name ?? "Someone",
                PartPrices = p.PartPrices.Select(pp => new PartPriceDto
                {
                    Price = pp.Price,
                    EffectiveFrom = pp.EffectiveFrom
                }).ToList()
            }).ToList();
            return partDtos;            
        }
        public async Task<List<Part>> GetZeroPartsAsync()
        {
            var parts = await _context.Parts
                .Where(p => p.Quantity <= 0)
                .ToListAsync();

            return parts;
        }

        public async Task<PartDto> GetPartByIdAsync(int partId)
        {
            var p = await _context.Parts
                 .Where(p => p.PartId == partId)
                .Include(p => p.PartPrices)
                .Include(p => p.WarrantyClaims)
                    .ThenInclude(w => w.Supplier).FirstOrDefaultAsync();
            var partDtos = new PartDto
            {
                PartId = p.PartId,
                PartName = p.PartName,
                Quantity = p.Quantity,
                Unit = p.Unit,
                CurrentUnitPrice = p.UnitPrice,
                ExpiryDate = p.ExpiryDate,
                WarrantyMonths = p.WarrantyMonths,
                SupplierName = p.WarrantyClaims.FirstOrDefault()?.Supplier?.Name ?? "Someone",
                PartPrices = p.PartPrices.Select(pp => new PartPriceDto
                {
                    Price = pp.Price,
                    EffectiveFrom = pp.EffectiveFrom
                }).ToList()
            };
            return partDtos;            
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


        

            public async Task<Part> CreatePartAsync(CreatePartDto dto)
            {
                var part = new Part
                {
                    PartName = dto.PartName,
                    Quantity = dto.Quantity,
                    UnitPrice = dto.UnitPrice,
                    ExpiryDate = dto.ExpiryDate,
                    WarrantyMonths = dto.WarrantyMonths,
                    Unit = dto.Unit
                };

                foreach (var priceDto in dto.PartPrices)
                {
                    var partPrice = new PartPrice
                    {
                        Price = priceDto.Price,
                        EffectiveFrom = priceDto.EffectiveFrom
                    };
                    part.PartPrices.Add(partPrice);
                }

                foreach (var claimDto in dto.WarrantyClaims)
                {
                    var warrantyClaim = new WarrantyClaim
                    {
                        SupplierId = claimDto.SupplierId,
                        ClaimDate = claimDto.ClaimDate,
                        Notes = claimDto.Notes
                    };
                    part.WarrantyClaims.Add(warrantyClaim);
                }

                _context.Parts.Add(part);
                await _context.SaveChangesAsync();

                return part;
            }

            public async Task<Part> UpdatePartAsync(int partId, UpdatePartDto dto)
            {
                var part = await _context.Parts
                    .Include(p => p.PartPrices)
                    .Include(p => p.WarrantyClaims)
                    .FirstOrDefaultAsync(p => p.PartId == partId);

                if (part == null)
                {
                    throw new Exception("Part not found.");
                }

                part.PartName = dto.PartName;
                part.Quantity = dto.Quantity;
                part.UnitPrice = dto.UnitPrice;
                part.ExpiryDate = dto.ExpiryDate;
                part.WarrantyMonths = dto.WarrantyMonths;
                part.Unit = dto.Unit;


                part.PartPrices.Clear(); 
                foreach (var priceDto in dto.PartPrices)
                {
                    var partPrice = new PartPrice
                    {
                        Price = priceDto.Price,
                        EffectiveFrom = priceDto.EffectiveFrom
                    };
                    part.PartPrices.Add(partPrice);
                }


                part.WarrantyClaims.Clear(); 
                foreach (var claimDto in dto.WarrantyClaims)
                {
                    var warrantyClaim = new WarrantyClaim
                    {
                        SupplierId = claimDto.SupplierId,
                        ClaimDate = claimDto.ClaimDate,
                        Notes = claimDto.Notes
                    };
                    part.WarrantyClaims.Add(warrantyClaim);
                }

                await _context.SaveChangesAsync();
                return part;
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
            /*var part = await GetPartByIdAsync(partId);
           if (part == null)
           {
               return null; // or throw an exception
           }
           part.PartName = partName;
           part.Quantity = quantity;
           part.CurrentUnitPrice = unitPrice;
           part.ExpiryDate = expiryDate;
           part.WarrantyMonths = warrantyMonths;
           _context.Parts.Update(part);
           await _context.SaveChangesAsync();
           return part;*/
            return null;
        }

        public async Task<RevenueReportDto> GenerateRevenueReport(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var orders = await _context.Orders
                .Include(o => o.Payments)
                .Include(o => o.OrderDetails)
                .Include(o => o.OrderDetails)
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .ToListAsync();

            var totalRevenue = orders.Sum(o => o.Payments.Sum(p => p.Amount) ?? 0);
            var totalOrders = orders.Count;

            var orderDetails = orders.Select(o => new OrderDetailDto
            {
                OrderId = o.OrderId,
                CreatedAt = o.CreatedAt,
                TotalAmount = o.Payments.Sum(p => p.Amount) ?? 0,
                LineItems = o.OrderDetails.Select(od => new OrderLineItemDto
                {
                    Item = (int)(od.ServiceId != null ? od.ServiceId : od.PackageId),
                    Quantity = od.Quantity ?? 1,
                    UnitPrice = od.UnitPrice ?? 0,
                    LineTotal = od.LineTotal ?? 0
                }).ToList()
            }).ToList();

            return new RevenueReportDto
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                OrderDetails = orderDetails
            };
        }


        public async Task<DashboardSummaryDto> GenerateDashboardSummary(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var appointments = await _context.Appointments
                .Include(a => a.AppointmentServices)
                    .ThenInclude(s => s.Service)
                .Include(a => a.Package)
                .Where(a => a.AppointmentDate >= startDate && a.AppointmentDate <= endDate)
                .ToListAsync();

            var partsUsage = new Dictionary<string, int>();
            var servicesCount = new Dictionary<string, int>(); 
            var packagesCount = new Dictionary<string, int>();

            foreach (var appointment in appointments)
            {
                // Count packages
                if (appointment.PackageId.HasValue)
                {
                    var packageName = appointment.Package?.Name ?? "Unknown Package";
                    if (packagesCount.ContainsKey(packageName))
                    {
                        packagesCount[packageName]++;
                    }
                    else
                    {
                        packagesCount[packageName] = 1;
                    }
                }

                foreach (var appointmentService in appointment.AppointmentServices)
                {
                    var serviceName = appointmentService.Service?.Name ?? "Unknown Service";
                    if (servicesCount.ContainsKey(serviceName))
                    {
                        servicesCount[serviceName]++;
                    }
                    else
                    {
                        servicesCount[serviceName] = 1;
                    }
                    var serviceParts = await _context.ServiceParts
                        .Include(sp => sp.Part)
                        .Where(sp => sp.ServiceId == appointmentService.ServiceId)
                        .ToListAsync();

                    foreach (var servicePart in serviceParts)
                    {
                        var partName = servicePart.Part?.PartName ?? "Unknown Part";
                        if (partsUsage.ContainsKey(partName))
                        {
                            partsUsage[partName] += servicePart.QuantityRequired;
                        }
                        else
                        {
                            partsUsage[partName] = servicePart.QuantityRequired;
                        }
                    }
                }
            }

            return new DashboardSummaryDto
            {
                PartsUsage = partsUsage,
                ServicesCount = servicesCount,
                PackagesCount = packagesCount
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
            appointment.Status = dto.Status;
            
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
                        Title = "Low stock alert",
                        Type = "Alert",                        
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

        public async Task DeletePartAsync(int partId)
        {
            var part = await _context.Parts
            .Include(p => p.ServiceParts)
                .ThenInclude(a => a.Service)
            .FirstOrDefaultAsync(p => p.PartId == partId);

            if (part == null)
            {
                throw new Exception("Part not found.");
            }
            foreach (var servicePart in part.ServiceParts)
            {
                var service = await _context.Services
                     .Include(s => s.AppointmentServices)
                         .ThenInclude(a => a.Appointment)
                         .ThenInclude(a => a.ServiceProgresses)
                     .FirstOrDefaultAsync(s => s.ServiceId == servicePart.ServiceId);
                if (service == null)
                {
                    throw new Exception("Service not found.");
                }

                var hasInProgressAppointments = service.AppointmentServices
                    .Any(a => a.Appointment.ServiceProgresses
                        .Any(sp => sp.Status != "Completed"));
                if (hasInProgressAppointments)
                {
                    throw new Exception("Cannot delete this part! It is being used in another service. To delete this part, any appointment with this service has to be completed!");
                }
            }                        
            _context.Parts.Remove(part);
            await _context.SaveChangesAsync();
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
