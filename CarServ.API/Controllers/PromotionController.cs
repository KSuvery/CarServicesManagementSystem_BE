using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Payment;
using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarServ.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpPost("create-promotion")]
        [Authorize(Roles = "1,2")] 
        public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionDto dto)
        {
            try
            {
                var promotion = await _promotionService.CreatePromotionAsync(dto);
                return CreatedAtAction(nameof(GetPromotionById), new { promotionId = promotion.PromotionId }, promotion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-promotion/{promotionId}")]
        [Authorize(Roles = "1,2")] 
        public async Task<IActionResult> UpdatePromotion(int promotionId, [FromBody] UpdatePromotionDto dto)
        {
            try
            {
                var updatedPromotion = await _promotionService.UpdatePromotionAsync(promotionId, dto);
                return Ok(updatedPromotion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("retrieve-all-promotion")]
        [Authorize(Roles = "1,2")] 
        public async Task<PaginationResult<List<Promotion>>> GetAllPromotions(int currentPage = 1, int pageSize = 5)
        {
            
                return await _promotionService.GetAllPromotionsWithPaging(currentPage, pageSize);
                           
        }

        [HttpGet("retrieve-promotion/{promotionId}")]
        [Authorize(Roles = "1,2")] 
        public async Task<IActionResult> GetPromotionById(int promotionId)
        {
            try
            {
                var promotion = await _promotionService.GetPromotionByIdAsync(promotionId);
                return Ok(promotion);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
