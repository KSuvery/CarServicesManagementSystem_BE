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
    public class AppointmentRepository : GenericRepository<Appointments>, IAppointmentRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public AppointmentRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Appointments>> GetAllAppointmentsAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        public async Task<Appointments> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

        public async Task<List<Appointments>> GetAppointmentsByCustomerIdAsync(int customerId)
        {
            return await _context.Appointments
                .Where(a => a.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Appointments>> GetAppointmentsByVehicleIdAsync(int vehicleId)
        {
            return await _context.Appointments
                .Where(a => a.VehicleId == vehicleId)
                .ToListAsync();
        }

        public async Task<Appointments> ScheduleAppointmentAsync(
            int customerId,
            int vehicleId,
            int packageId,
            DateTime appointmentDate,
            string status = "Pending",
            int? promotionId = null)
        {
            var appointment = new Appointments
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

        public async Task<Appointments> UpdateAppointmentAsync(
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
