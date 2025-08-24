using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Payment;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _repository;

        public PromotionService(IPromotionRepository repository)
        {
            _repository = repository;
        }
        public async Task<Promotion> CreatePromotionAsync(CreatePromotionDto dto)
        {
            return await _repository.CreatePromotionAsync(dto);
        }

        public async Task<List<Promotion>> GetAllPromotionsAsync()
        {
            return await _repository.GetAllPromotionsAsync();
        }

        public async Task<PaginationResult<List<Promotion>>> GetAllPromotionsWithPaging(int currentPage, int pageSize)
        {
            return await _repository.GetAllPromotionsWithPaging(currentPage, pageSize);
        }

        public async Task<Promotion> GetPromotionByIdAsync(int promotionId)
        {
            return await _repository.GetPromotionByIdAsync(promotionId);    
        }

        public async Task<Promotion> UpdatePromotionAsync(int promotionId, UpdatePromotionDto dto)
        {
            return await _repository.UpdatePromotionAsync(promotionId, dto);
        }
    }
}
