using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<List<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);
        Task<List<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId);
        Task<List<Appointment>> GetAppointmentsByVehicleIdAsync(int vehicleId);
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
