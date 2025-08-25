using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<List<AppointmentDto>> GetAllAppointmentsAsync();
        Task<PaginationResult<List<AppointmentDto>>> GetAllApppointmentsWithPaging(int currentPage, int pageSize);
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);
        Task<Appointment> GetAppointmentByOrderIdAsync(int orderId);
        Task<List<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId);
        Task<List<Appointment>> GetBookedAppointmentsByCustomerId(int customerid);
        Task<List<Appointment>> GetAppointmentsByVehicleIdAsync(int vehicleId);
        Task<Appointment> ScheduleAppointment(int customerId, ScheduleAppointmentDto dto);
        Task<Appointment> ScheduleAppointmentAsync(
            int customerId,
            int vehicleId,
            int packageId,
            DateTime appointmentDate,
            string status = "Pending",
            int? promotionId = null);
        Task<Appointment> UpdateAppointmentAsync(
            int appointmentId,
            string status);
    }
}
