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
    public class CarTypesService : ICarTypesService
    {
        private readonly ICarTypesRepository _carTypesRepository;
        public CarTypesService(ICarTypesRepository carTypesRepository)
        {
            _carTypesRepository = carTypesRepository;
        }

        public async Task<List<CarTypes>> GetAllCarTypesAsync()
        {
            return await _carTypesRepository.GetAllCarTypesAsync();
        }

        public async Task<CarTypes> GetCarTypeByIdAsync(int carTypeId)
        {
            return await _carTypesRepository.GetCarTypeByIdAsync(carTypeId);
        }

        public async Task<List<CarTypes>> GetCarTypesByNameAsync(string typeName)
        {
            return await _carTypesRepository.GetCarTypesByNameAsync(typeName);
        }

        public async Task<CarTypes> AddCarTypeAsync(string typeName, string description)
        {
            return await _carTypesRepository.AddCarTypeAsync(typeName, description);
        }

        public async Task<CarTypes> UpdateCarTypeAsync(int carTypeId, string typeName, string description)
        {
            return await _carTypesRepository.UpdateCarTypeAsync(carTypeId, typeName, description);
        }
    }
}
