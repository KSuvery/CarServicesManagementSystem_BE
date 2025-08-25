using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.DTO.Logging_part_usage;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace CarServ.Repository.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public VehicleRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<VehicleDto>> GetAllVehiclesAsync()
        {
            var vehicles = await _context.Vehicles
            .Include(v => v.Customer)
            .ThenInclude(c => c.User)
                .ToListAsync();
            var vehicleDtos = vehicles.Select(p => new VehicleDto
            {
                VehicleId = p.VehicleId,
                CustomerName = p.Customer.User.FullName,
               LicensePlate = p.LicensePlate,
               Make = p.Make,
               Model = p.Model,
               Year = p.Year
            }).ToList();
            return vehicleDtos;
        }

        public async Task<VehicleDto> GetVehicleByIdAsync(int id)
        {
            var vehicle = await _context.Vehicles
            .Include(v => v.Customer)
            .ThenInclude(c => c.User)
            .Where(v => v.VehicleId == id)
                .FirstOrDefaultAsync();
            var vehicleDtos =  new VehicleDto
            {
                VehicleId = vehicle.VehicleId,
                CustomerName = vehicle.Customer.User.FullName,
                LicensePlate = vehicle.LicensePlate,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year
            };
            return vehicleDtos;
        }

        public async Task<List<VehicleDto>> GetVehiclesByCustomerIdAsync(int customerId)
        {
            var vehicles = await _context.Vehicles
           .Include(v => v.Customer)
           .ThenInclude(c => c.User)
           .Where(v => v.CustomerId == customerId)
               .ToListAsync();
            var vehicleDtos = vehicles.Select(p => new VehicleDto
            {
                VehicleId = p.VehicleId,
                CustomerName = p.Customer.User.FullName,
                LicensePlate = p.LicensePlate,
                Make = p.Make,
                Model = p.Model,
                Year = p.Year
            }).ToList();
            return vehicleDtos;
        }

        public async Task<List<VehicleDto>> GetVehiclesByMakeAsync(string make)
        {
            var vehicles = await _context.Vehicles
           .Include(v => v.Customer)
           .ThenInclude(c => c.User)
           .Where(v => v.Make == make)
               .ToListAsync();
            var vehicleDtos = vehicles.Select(p => new VehicleDto
            {VehicleId = p.VehicleId,
                CustomerName = p.Customer.User.FullName,
                LicensePlate = p.LicensePlate,
                Make = p.Make,
                Model = p.Model,
                Year = p.Year
            }).ToList();
            return vehicleDtos;
        }

        public async Task<VehicleDto> GetVehicleByLicensePlateAsync(string licensePlate)
        {
            var vehicle = await _context.Vehicles
            .Include(v => v.Customer)
            .ThenInclude(c => c.User)
            .Where(v => v.LicensePlate == licensePlate)
                .FirstOrDefaultAsync();
            var vehicleDtos = new VehicleDto
            {VehicleId = vehicle.VehicleId,
                CustomerName = vehicle.Customer.User.FullName,
                LicensePlate = vehicle.LicensePlate,
                Make = vehicle.Make,
                Model = vehicle.Model,
                Year = vehicle.Year
            };
            return vehicleDtos;
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

        public async Task<List<Vehicle>> GetBookedVehiclesAsync(string status = "Booked")
        {
            return await _context.Vehicles
                .Where(v => v.Status.ToLower() == status.ToLower())
                .ToListAsync();
        }

        public async Task<List<Vehicle>> GetAvailableVehiclesAsync(string status = "Available")
        {
            return await _context.Vehicles
                .Where(v => v.Status.ToLower() == status.ToLower())
                .ToListAsync();
        }

        public async Task<List<Vehicle>> GetInServiceVehiclesAsync(string status = "In Service")
        {
            return await _context.Vehicles
                .Where(v => v.Status.ToLower() == status.ToLower())
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
