using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarServ.Repository.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public VehicleRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Vehicle>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicle> GetVehicleByIdAsync(int id)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == id);
        }

        public async Task<List<Vehicle>> GetVehiclesByCustomerIdAsync(int customerId)
        {
            return await _context.Vehicles
                .Where(v => v.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Vehicle>> GetVehiclesByMakeAsync(string make)
        {
            return await _context.Vehicles
                .Where(v => v.Make.ToLower().Contains(make.ToLower()))
                .ToListAsync();
        }

        public async Task<Vehicle> GetVehicleByLicensePlateAsync(string licensePlate)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.LicensePlate.ToLower() == licensePlate.ToLower());
        }

        public async Task<List<Vehicle>> GetVehiclesByModelAsync(string model)
        {
            return await _context.Vehicles
                .Where(v => v.Model.ToLower().Contains(model.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Vehicle>> GetVehiclesByYearRangeAsync(int minYear, int maxYear)
        {
            return await _context.Vehicles
                .Where(v => v.Year >= minYear && v.Year <= maxYear)
                .ToListAsync();
        }

        public async Task<List<Vehicle>> GetVehiclesByCarTypeIdAsync(int carTypeId)
        {
            return await _context.Vehicles
                .Where(v => v.CarTypeId == carTypeId)
                .ToListAsync();
        }


        public async Task<Vehicle> AddVehicleAsync(int customerId, AddVehicleDto dto)
        {
            // Validate the input data
            if (string.IsNullOrEmpty(dto.LicensePlate))
            {
                throw new ArgumentException("License plate is required.");
            }

            if (string.IsNullOrEmpty(dto.Make))
            {
                throw new ArgumentException("Make is required.");
            }

            if (string.IsNullOrEmpty(dto.Model))
            {
                throw new ArgumentException("Model is required.");
            }

            // Create a new vehicle
            var vehicle = new Vehicle
            {
                CustomerId = customerId,
                LicensePlate = dto.LicensePlate,
                Make = dto.Make,
                Model = dto.Model,
                Year = dto.Year,
                CarTypeId = dto.CarTypeId
            };

            // Add the vehicle to the context
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync(); // Save changes to the database

            return vehicle;
        }

        public async Task<bool> RemoveVehicleAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null)
            {
                return false;
            }
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
