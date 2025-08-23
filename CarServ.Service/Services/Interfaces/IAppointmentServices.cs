using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services.Interfaces
{
    public interface IAppointmentervices
    {
        Task<List<AppointmentDto>> GetAllAppointmentAsync();
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);
        Task<List<Appointment>> GetAppointmentByCustomerIdAsync(int customerId);
        Task<List<Appointment>> GetAppointmentByVehicleIdAsync(int vehicleId);
        Task<Appointment> ScheduleAppointment(int customerId, ScheduleAppointmentDto dto);
        Task<Appointment> ScheduleAppointmentAsync(int customerId, int vehicleId, int packageId, DateTime appointmentDate, string status = "Pending", int? promotionId = null);
        Task<Appointment> UpdateAppointmentAsync(
            int appointmentId,
            string status);
    }
}
