using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories
{
    public class VehicleRepository : GenericRepository<Vehicles>, IVehicleRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public VehicleRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Vehicles>> GetAllVehiclesAsync()
        {
            return await _context.Vehicles.ToListAsync();
        }

        public async Task<Vehicles> GetVehicleByIdAsync(int id)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.VehicleId == id);
        }

        public async Task<List<Vehicles>> GetVehiclesByCustomerIdAsync(int customerId)
        {
            return await _context.Vehicles
                .Where(v => v.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Vehicles>> GetVehiclesByMakeAsync(string make)
        {
            return await _context.Vehicles
                .Where(v => v.Make.ToLower().Contains(make.ToLower()))
                .ToListAsync();
        }

        public async Task<Vehicles> GetVehicleByLicensePlateAsync(string licensePlate)
        {
            return await _context.Vehicles
                .FirstOrDefaultAsync(v => v.LicensePlate.ToLower() == licensePlate.ToLower());
        }

        public async Task<List<Vehicles>> GetVehiclesByModelAsync(string model)
        {
            return await _context.Vehicles
                .Where(v => v.Model.ToLower().Contains(model.ToLower()))
                .ToListAsync();
        }

        public async Task<List<Vehicles>> GetVehiclesByYearRangeAsync(int minYear, int maxYear)
        {
            return await _context.Vehicles
                .Where(v => v.Year >= minYear && v.Year <= maxYear)
                .ToListAsync();
        }

        public async Task<List<Vehicles>> GetVehiclesByCarTypeIdAsync(int carTypeId)
        {
            return await _context.Vehicles
                .Where(v => v.CarTypeId == carTypeId)
                .ToListAsync();
        }

        //Create a vehicle with defined customerId corresponding to the customer logged in

    }
}
