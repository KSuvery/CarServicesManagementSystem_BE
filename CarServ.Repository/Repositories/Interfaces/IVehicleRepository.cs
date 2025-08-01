using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IVehicleRepository : IGenericRepository<Vehicles>
    {
        Task<List<Vehicles>> GetAllVehiclesAsync();
        Task<Vehicles> GetVehicleByIdAsync(int id);
        Task<List<Vehicles>> GetVehiclesByCustomerIdAsync(int customerId);
        Task<List<Vehicles>> GetVehiclesByMakeAsync(string make);
        Task<Vehicles> GetVehicleByLicensePlateAsync(string licensePlate);
        Task<List<Vehicles>> GetVehiclesByModelAsync(string model);
        Task<List<Vehicles>> GetVehiclesByYearRangeAsync(int minYear, int maxYear);
        Task<List<Vehicles>> GetVehiclesByCarTypeIdAsync(int carTypeId);
    }
}
