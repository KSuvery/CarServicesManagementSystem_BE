using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointments>
    {
        Task<List<Appointments>> GetAllAppointmentsAsync();
        Task<Appointments> GetAppointmentByIdAsync(int appointmentId);
        Task<List<Appointments>> GetAppointmentsByCustomerIdAsync(int customerId);
        Task<List<Appointments>> GetAppointmentsByVehicleIdAsync(int vehicleId);
        Task<Appointments> ScheduleAppointmentAsync(
            int customerId,
            int vehicleId,
            int packageId,
            DateTime appointmentDate,
            string status = "Pending",
            int? promotionId = null);
        Task<Appointments> UpdateAppointmentAsync(
            int appointmentId,
            string status);
    }
}
