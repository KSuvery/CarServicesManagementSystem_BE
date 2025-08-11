using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface ICarTypesService
    {
        Task<List<CarType>> GetAllCarTypesAsync();
        Task<CarType> GetCarTypeByIdAsync(int carTypeId);
        Task<List<CarType>> GetCarTypesByNameAsync(string typeName);
        Task<CarType> AddCarTypeAsync(string typeName, string description);
        Task<CarType> UpdateCarTypeAsync(int carTypeId, string typeName, string description);
    }
}
