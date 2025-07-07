using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentServices(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<Appointments>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAllAppointmentsAsync();
        }

        public async Task<Appointments> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
        }

        public async Task<List<Appointments>> GetAppointmentsByCustomerIdAsync(int customerId)
        {
            return await _appointmentRepository.GetAppointmentsByCustomerIdAsync(customerId);
        }

        public async Task<List<Appointments>> GetAppointmentsByVehicleIdAsync(int vehicleId)
        {
            return await _appointmentRepository.GetAppointmentsByVehicleIdAsync(vehicleId);
        }

        public async Task<Appointments> ScheduleAppointmentAsync(
            int customerId,
            int vehicleId,
            int packageId,
            DateTime appointmentDate,
            string status = "Pending",
            int? promotionId = null)
        {
            return await _appointmentRepository.ScheduleAppointmentAsync(
                customerId,
                vehicleId,
                packageId,
                appointmentDate,
                status,
                promotionId);
        }

        public async Task<Appointments> UpdateAppointmentAsync(
            int appointmentId,
            string status)
        {
            return await _appointmentRepository.UpdateAppointmentAsync(appointmentId, status);
        }
    }
}
