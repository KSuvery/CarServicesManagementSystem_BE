using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Payment;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CarServ.Repository.Repositories.Interfaces.PromotionRepository;

namespace CarServ.Repository.Repositories.Interfaces
{
    public class PromotionRepository : IPromotionRepository
    {
        
            private readonly CarServicesManagementSystemContext _context;

            public PromotionRepository(CarServicesManagementSystemContext context)
            {
                _context = context;
            }

            public async Task<Promotion> CreatePromotionAsync(CreatePromotionDto dto)
            {
                var promotion = new Promotion
                {
                    Title = dto.Title,
                    DiscountPercentage = dto.DiscountPercentage,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate
                };

                _context.Promotions.Add(promotion);
                await _context.SaveChangesAsync();

                return promotion;
            }

            public async Task<Promotion> UpdatePromotionAsync(int promotionId, UpdatePromotionDto dto)
            {
                var promotion = await _context.Promotions.FindAsync(promotionId);

                if (promotion == null)
                {
                    throw new Exception("Promotion not found.");
                }

                // Update promotion properties
                promotion.Title = dto.Title;
                promotion.DiscountPercentage = dto.DiscountPercentage;
                promotion.StartDate = dto.StartDate;
                promotion.EndDate = dto.EndDate;

                await _context.SaveChangesAsync();
                return promotion;
            }

            public async Task<List<Promotion>> GetAllPromotionsAsync()
            {
                return await _context.Promotions.ToListAsync();
            }


        public async Task<PaginationResult<List<Promotion>>> GetAllPromotionsWithPaging(int currentPage, int pageSize)
        {
            var userListTmp = await this.GetAllPromotionsAsync();

            var totalItems = userListTmp.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            userListTmp = userListTmp.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationResult<List<Promotion>>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Items = userListTmp
            };
        }

        public async Task<Promotion> GetPromotionByIdAsync(int promotionId)
            {
                var promotion = await _context.Promotions.FindAsync(promotionId);
                if (promotion == null)
                {
                    throw new Exception("Promotion not found.");
                }
                return promotion;
            }

        /*public async Task<Promotion> UpdateStatusAsync(int Id, bool status)
        {
  
            var user = await _context.Promotions.FindAsync(Id);

            if (user == null)
            {
                throw new Exception("User not found.");
            }


            user. = status;

            // Save changes to the database
            await _context.SaveChangesAsync();

            return user;
        }*/
    }
}
