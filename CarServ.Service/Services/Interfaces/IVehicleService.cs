using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle> GetVehicleByIdAsync(int id);
        Task<List<Vehicle>> GetVehiclesByCustomerIdAsync(int customerId);
        Task<List<Vehicle>> GetVehiclesByMakeAsync(string make);
        Task<Vehicle> GetVehicleByLicensePlateAsync(string licensePlate);
        Task<List<Vehicle>> GetVehiclesByModelAsync(string model);
        Task<List<Vehicle>> GetVehiclesByYearRangeAsync(int minYear, int maxYear);
        Task<List<Vehicle>> GetVehiclesByCarTypeIdAsync(int carTypeId);
        Task<List<Vehicle>> GetBookedVehiclesAsync(string status = "Booked");
        Task<List<Vehicle>> GetAvailableVehiclesAsync(string status = "Available");
        Task<Vehicle> AddVehicleAsync(int customerId, AddVehicleDto dto);
        Task<bool> RemoveVehicleAsync(int vehicleId);
    }
}
