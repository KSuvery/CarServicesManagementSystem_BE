using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarServ.Repository.Repositories.AppointmentRepository;

namespace CarServ.Repository.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public AppointmentRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<AppointmentDto>> GetAllAppointmentsAsync()
        {
            var appointments = await _context.Appointments
                .Include(a => a.AppointmentServices)
                    .ThenInclude(s => s.Service)
                .Include(a => a.Customer)
                .Include(a => a.Package)
                    .ThenInclude(p => p.Services)
                .Select(a => new AppointmentDto
                {
                    AppointmentId = a.AppointmentId,
                    CustomerName = a.Customer.User.FullName, // Updated to use 'User' navigation property
                    CustomerPhone = a.Customer.User.PhoneNumber, // Updated to use 'User' navigation property
                    CustomerAddress = a.Customer.User.Address, // Updated to use 'User' navigation property
                    VehicleLicensePlate = a.Vehicle.LicensePlate,
                    VehicleMake = a.Vehicle.Make,
                    VehicleModel = a.Vehicle.Model,
                    services = a.AppointmentServices.Select(s => s.Service.Name).ToList(),
                    Duration = (int)(a.AppointmentServices.Sum(s => s.Service.EstimatedLaborHours ?? 0) +
                               (a.Package != null ? a.Package.Services.Sum(s => s.EstimatedLaborHours ?? 0) : 0)),
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status
                })
                .ToListAsync();

            return appointments;
        }

        public async Task<PaginationResult<List<AppointmentDto>>> GetAllApppointmentsWithPaging(int currentPage, int pageSize)
        {
            var userListTmp = await this.GetAllAppointmentsAsync();

            var totalItems = userListTmp.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            userListTmp = userListTmp.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationResult<List<AppointmentDto>>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Items = userListTmp
            };
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<Appointment> GetAppointmentByOrderIdAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Appointment)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            return order?.Appointment;
        }

        public async Task<List<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId)
        {
            return await _context.Appointments
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetBookedAppointmentsByCustomerId(int customerid)
        {
            return await _context.Appointments
                .Where(a => a.CustomerId == customerid && a.Status == "Booked")
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByVehicleIdAsync(int vehicleId)
        {
            return await _context.Appointments
                .Where(a => a.VehicleId == vehicleId)
                .ToListAsync();
        }

        public async Task<Appointment> ScheduleAppointmentAsync(
            int customerId,
            int vehicleId,
            int packageId,
            DateTime appointmentDate,
            string status = "Pending",
            int? promotionId = null)
        {
            var appointment = new Appointment
            {
                CustomerId = customerId,
                VehicleId = vehicleId,
                PackageId = packageId,
                AppointmentDate = appointmentDate,
                Status = status,
                PromotionId = promotionId
            };

            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;

        }

        private async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == id);
        }

        public async Task<Appointment> ScheduleAppointment(int customerId, ScheduleAppointmentDto dto)
            {
                if (dto.VehicleId == null)
                {
                    throw new ArgumentException("Vehicle must be selected.");
                }

                if (dto.AppointmentDate == default)
                {
                    throw new ArgumentException("Appointment date must be provided.");
                }

                // Check time
                var isAvailable = await CheckAvailability(dto.AppointmentDate, customerId);
                if (!isAvailable)
                {
                    throw new InvalidOperationException("Selected time is not available.");
                }

                var appointment = new Appointment
                {
                    CustomerId = customerId,
                    VehicleId = dto.VehicleId,
                    PackageId = dto.PackageId,
                    AppointmentDate = dto.AppointmentDate,
                    Status = "Booked",
                    PromotionId = dto.PromotionId
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();


            var vehicle = await GetVehicleByIdAsync((int)dto.VehicleId);            
            vehicle.Status = "In Service";
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
            // Create a new order as well
            var order = new Order
            {
                AppointmentId = appointment.AppointmentId,
                CreatedAt = DateTime.Now,
                PromotionId = dto.PromotionId // Link promotion if yes
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            if (dto.PackageId.HasValue)
            {
                var packageDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    PackageId = dto.PackageId,
                    Quantity = 1                    
        };
                _context.OrderDetails.Add(packageDetail);
            }
            //Retrieve services included in the selected package
            List<int> packageServiceIds = new List<int>();
            if (dto.PackageId.HasValue)
            {
                packageServiceIds = await _context.ServicePackages
                    .Where(sp => sp.PackageId == dto.PackageId.Value)
                    .Select(sp => sp.Services.Select(s => s.ServiceId)) // select IDs
                    .SelectMany(ids => ids) // flatten nested lists
                    .ToListAsync();
            }
            foreach (var serviceId in packageServiceIds)
            {
                
                
                    var appointmentService = new AppointmentService
                    {
                        AppointmentId = appointment.AppointmentId,
                        ServiceId = serviceId,
                        Quantity = 1
                    };
                    _context.AppointmentServices.Add(appointmentService);
                    // Also add the service detail to the order
                    var serviceDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ServiceId = serviceId,
                        Quantity = 1,
                    };
                    _context.OrderDetails.Add(serviceDetail);
                
            }

            // Add multiple services, skipping those already included in the package
            foreach (var serviceId in dto.ServiceIds)
            {
                if (!packageServiceIds.Contains(serviceId))
                {
                    var appointmentService = new AppointmentService
                    {
                        AppointmentId = appointment.AppointmentId,
                        ServiceId = serviceId,
                        Quantity = 1
                    };
                    _context.AppointmentServices.Add(appointmentService);
                    // Also add the service detail to the order
                    var serviceDetail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ServiceId = serviceId,
                        Quantity = 1,
                    };
                    _context.OrderDetails.Add(serviceDetail);
                }
            }
            /*//  a service package
            if (dto.PackageId.HasValue)
            {
                appointment.PackageId = dto.PackageId.Value;
            }

            // multiple services 
            foreach (var serviceId in dto.ServiceIds)
                {
                    var appointmentService = new AppointmentService
                    {
                        AppointmentId = appointment.AppointmentId,
                        ServiceId = serviceId,
                        Quantity = 1 
                    };
                    _context.AppointmentServices.Add(appointmentService);
                }*/


            var serviceProgress = new ServiceProgress
                {
                    AppointmentId = appointment.AppointmentId,
                    Status = "Booked",
                    Note = "Appointment confirmed for customer " + customerId,
                    UpdatedAt = DateTime.Now
                };

                _context.ServiceProgresses.Add(serviceProgress);

             
                await _context.SaveChangesAsync();

                return appointment;
            }

            private async Task<bool> CheckAvailability(DateTime appointmentDate, int customerId)
            {
                // if there are any existing appointments for the same time
                var existingAppointments = await _context.Appointments
                    .AnyAsync(a => a.AppointmentDate == appointmentDate && a.CustomerId == customerId);

                return !existingAppointments;
            }
        


        public async Task<Appointment> UpdateAppointmentAsync(
            int appointmentId,
            string status)
        {
            var appointment = await GetAppointmentByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");
            }
            appointment.Status = status;
            await UpdateAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
    }
}
