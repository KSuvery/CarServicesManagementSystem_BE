using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;

namespace CarServ.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<List<VehicleDto>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllVehiclesAsync();
        }

        public async Task<VehicleDto> GetVehicleByIdAsync(int id)
        {
            return await _vehicleRepository.GetVehicleByIdAsync(id);
        }

        public async Task<List<VehicleDto>> GetVehiclesByCustomerIdAsync(int customerId)
        {
            return await _vehicleRepository.GetVehiclesByCustomerIdAsync(customerId);
        }

        public async Task<List<VehicleDto>> GetVehiclesByMakeAsync(string make)
        {
            return await _vehicleRepository.GetVehiclesByMakeAsync(make);
        }

        public async Task<VehicleDto> GetVehicleByLicensePlateAsync(string licensePlate)
        {
            return await _vehicleRepository.GetVehicleByLicensePlateAsync(licensePlate);
        }

        public async Task<List<Vehicle>> GetVehiclesByModelAsync(string model)
        {
            return await _vehicleRepository.GetVehiclesByModelAsync(model);
        }

        public async Task<List<Vehicle>> GetVehiclesByYearRangeAsync(int minYear, int maxYear)
        {
            return await _vehicleRepository.GetVehiclesByYearRangeAsync(minYear, maxYear);
        }

        public async Task<List<Vehicle>> GetVehiclesByCarTypeIdAsync(int carTypeId)
        {
            return await _vehicleRepository.GetVehiclesByCarTypeIdAsync(carTypeId);
        }

        public async Task<List<Vehicle>> GetBookedVehiclesAsync(string status = "Booked")
        {
            return await _vehicleRepository.GetBookedVehiclesAsync(status);
        }

        public async Task<List<Vehicle>> GetAvailableVehiclesAsync(string status = "Available")
        {
            return await _vehicleRepository.GetAvailableVehiclesAsync(status);
        }

        public async Task<List<Vehicle>> GetInServiceVehiclesAsync(string status = "In Service")
        {
            return await _vehicleRepository.GetInServiceVehiclesAsync(status);
        }

        public async Task<Vehicle> AddVehicleAsync(int customerId, AddVehicleDto dto)
        {
            return await _vehicleRepository.AddVehicleAsync(customerId, dto);
        }

        public async Task<bool> RemoveVehicleAsync(int vehicleId)
        {
            return await _vehicleRepository.RemoveVehicleAsync(vehicleId);
        }
    }
}
