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
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<List<VehicleDto>> GetAllVehiclesAsync();
        Task<VehicleDto> GetVehicleByIdAsync(int id);
        Task<List<VehicleDto>> GetVehiclesByCustomerIdAsync(int customerId);
        Task<List<VehicleDto>> GetVehiclesByMakeAsync(string make);
        Task<VehicleDto> GetVehicleByLicensePlateAsync(string licensePlate);
        Task<List<Vehicle>> GetVehiclesByModelAsync(string model);
        Task<List<Vehicle>> GetVehiclesByYearRangeAsync(int minYear, int maxYear);
        Task<List<Vehicle>> GetVehiclesByCarTypeIdAsync(int carTypeId);
        Task<List<Vehicle>> GetBookedVehiclesAsync(string status = "Booked");
        Task<List<Vehicle>> GetAvailableVehiclesAsync(string status = "Available");
        Task<List<Vehicle>> GetInServiceVehiclesAsync(string status = "In Service");
        Task<Vehicle> AddVehicleAsync(int customerId, AddVehicleDto dto);
        Task<bool> RemoveVehicleAsync(int vehicleId);
    }
}
