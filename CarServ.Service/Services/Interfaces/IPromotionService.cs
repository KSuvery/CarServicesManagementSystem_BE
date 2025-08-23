using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<Promotion> GetPromotionByIdAsync(int promotionId);
        Task<List<Promotion>> GetAllPromotionsAsync();
        Task<Promotion> UpdatePromotionAsync(int promotionId, UpdatePromotionDto dto);
        Task<Promotion> CreatePromotionAsync(CreatePromotionDto dto);
    }
}
