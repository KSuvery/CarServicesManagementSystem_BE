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
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            return await _vehicleRepository.GetAllVehiclesAsync();
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            return await _vehicleRepository.GetVehicleByIdAsync(id);
        }

        public async Task<List<Vehicle>> GetVehiclesByCustomerIdAsync(int customerId)
        {
            return await _vehicleRepository.GetVehiclesByCustomerIdAsync(customerId);
        }

        public async Task<List<Vehicle>> GetVehiclesByMakeAsync(string make)
        {
            return await _vehicleRepository.GetVehiclesByMakeAsync(make);
        }

        public async Task<Vehicle> GetVehicleByLicensePlateAsync(string licensePlate)
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
    }
}
