using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services
{
    public class Appointmentervices : IAppointmentervices
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public Appointmentervices(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<AppointmentDto>> GetAllAppointmentAsync()
        {
            return await _appointmentRepository.GetAllAppointmentsAsync();
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            return await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
        }

        public async Task<Appointment> GetAppointmentByOrderIdAsync(int orderId)
        {
            return await _appointmentRepository.GetAppointmentByOrderIdAsync(orderId);
        }

        public async Task<List<Appointment>> GetAppointmentByCustomerIdAsync(int customerId)
        {
            return await _appointmentRepository.GetAppointmentsByCustomerIdAsync(customerId);
        }

        public async Task<List<Appointment>> GetBookedAppointmentsByCustomerId(int customerid)
        {
            return await _appointmentRepository.GetBookedAppointmentsByCustomerId(customerid);
        }

        public async Task<List<Appointment>> GetAppointmentByVehicleIdAsync(int vehicleId)
        {
            return await _appointmentRepository.GetAppointmentsByVehicleIdAsync(vehicleId);
        }

        public async Task<Appointment> ScheduleAppointmentAsync(
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

        public async Task<Appointment> UpdateAppointmentAsync(
            int appointmentId,
            string status)
        {
            return await _appointmentRepository.UpdateAppointmentAsync(appointmentId, status);
        }

        public async Task<Appointment> ScheduleAppointment(int customerId, ScheduleAppointmentDto dto)
        {
            return await _appointmentRepository.ScheduleAppointment(customerId, dto);
        }

        public async Task<PaginationResult<List<AppointmentDto>>> GetAllApppointmentsWithPaging(int currentPage, int pageSize)
        {
            return await _appointmentRepository.GetAllApppointmentsWithPaging(currentPage, pageSize);
        }
    }
}
