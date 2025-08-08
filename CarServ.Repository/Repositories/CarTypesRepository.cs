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
    public class CarTypesRepository : GenericRepository<CarType>, ICarTypesRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public CarTypesRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CarType>> GetAllCarTypesAsync()
        {
            return await _context.CarTypes.ToListAsync();
        }

        public async Task<CarType> GetCarTypeByIdAsync(int carTypeId)
        {
            return await _context.CarTypes.FindAsync(carTypeId);
        }

        public async Task<List<CarType>> GetCarTypesByNameAsync(string typeName)
        {
            var carTypes = await _context.CarTypes
                .Where(ct => ct.TypeName.ToLower().Contains(typeName.ToLower()))
                .ToListAsync();

            return carTypes;
        }

        public async Task<CarType> AddCarTypeAsync(
            string typeName,
            string description)
        {
            var carType = new CarType
            {
                TypeName = typeName,
                Description = description
            };
            _context.CarTypes.Add(carType);
            await _context.SaveChangesAsync();
            return carType;
        }

        public async Task<CarType> UpdateCarTypeAsync(
            int carTypeId,
            string typeName,
            string description)
        {
            var carType = await _context.CarTypes.FindAsync(carTypeId);
            if (carType == null)
            {
                return null; // or throw an exception
            }
            carType.TypeName = typeName;
            carType.Description = description;
            _context.CarTypes.Update(carType);
            await _context.SaveChangesAsync();
            return carType;
        }   
    }
}
