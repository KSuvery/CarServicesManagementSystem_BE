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
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public AppointmentRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<List<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId)
        {
            return await _context.Appointments
                .Where(a => a.CustomerId == customerId)
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
            appointmentDate = DateTime.UtcNow;
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

        public async Task<Appointment> UpdateAppointmentAsync(
            int appointmentId,
            int customerId,
            int vehicleId,
            int packageId,
            DateTime appointmentDate,
            string status,
            int? promotionId)
        {
            var appointment = await GetAppointmentByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");
            }
            appointment.CustomerId = customerId;
            appointment.VehicleId = vehicleId;
            appointment.PackageId = packageId;
            appointment.AppointmentDate = appointmentDate;
            appointment.Status = status;
            appointment.PromotionId = promotionId;
            await UpdateAsync(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
    }
}
