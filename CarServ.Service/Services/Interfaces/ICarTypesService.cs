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
        Task<List<CarTypes>> GetAllCarTypesAsync();
        Task<CarTypes> GetCarTypeByIdAsync(int carTypeId);
        Task<List<CarTypes>> GetCarTypesByNameAsync(string typeName);
        Task<CarTypes> AddCarTypeAsync(string typeName, string description);
        Task<CarTypes> UpdateCarTypeAsync(int carTypeId, string typeName, string description);
    }
}
