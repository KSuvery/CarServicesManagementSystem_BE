using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Logging_part_usage;
using CarServ.Service.Services;
using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PartsController : ControllerBase
    {
        private readonly IPartsService _partsService;
        public PartsController(IPartsService partsService)
        {
            _partsService = partsService;
        }

        [HttpGet]
        [Authorize(Roles = "1,4")]
        public async Task<PaginationResult<List<PartDto>>> GetAllParts(int currentPage = 1, int pageSize = 5)
        {
            return await _partsService.GetAllPartsWithPaging(currentPage, pageSize);           
        }
        [HttpGet("suppliers")]
        [Authorize(Roles = "1,4")]
        public async Task<PaginationResult<List<Supplier>>> GetSuppliers(int currentPage = 1, int pageSize = 5)
        {
            return await _partsService.GetAllSuppliersAsync(currentPage, pageSize);
            
        }

        [HttpGet("get-low-parts")]
        [Authorize(Roles = "1,4")]
        public async Task<ActionResult<IEnumerable<PartDto>>> GetAllLowParts()
        {
            var parts = await _partsService.GetLowPartsAsync();
            return Ok(parts);
        }
        [HttpGet("get-out-of-stock-parts")]
        [Authorize(Roles = "1,4")]
        public async Task<ActionResult<IEnumerable<Part>>> GetZeroParts()
        {
            var parts = await _partsService.GetZeroPartsAsync();
            return Ok(parts);
        }

        [HttpGet("{partId}")]
        [Authorize(Roles = "1,4")]
        public async Task<IActionResult> GetPartById(int partId)
        {
            var part = await _partsService.GetPartByIdAsync(partId);
            if (part == null)
            {
                return NotFound();
            }
            return Ok(part);
        }

        [HttpGet("search")]
        [Authorize(Roles = "1,4")]
        public async Task<ActionResult<IEnumerable<Part>>> SearchParts([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty");
            }
            var parts = await _partsService.GetPartsByPartName(query);
            return Ok(parts);
        }

        [HttpGet("price")]
        [Authorize(Roles = "1,4")]
        public async Task<ActionResult<IEnumerable<Part>>> GetPartsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            if (minPrice < 0 || maxPrice < 0 || minPrice > maxPrice)
            {
                return BadRequest("Invalid price range");
            }
            var parts = await _partsService.GetPartsByUnitPriceRange(minPrice, maxPrice);
            return Ok(parts);
        }

        [HttpGet("expiryDate")]
        [Authorize(Roles = "1,4")]
        public async Task<ActionResult<IEnumerable<Part>>> GetPartsByExpiryDateRange([FromQuery] DateOnly startDate, DateOnly endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be after end date");
            }
            var parts = await _partsService.GetPartsByExpiryDateRange(startDate, endDate);
            return Ok(parts);
        }

        [HttpGet("warrantyMonths")]
        [Authorize(Roles = "1,4")]
        public async Task<ActionResult<IEnumerable<Part>>> GetPartsByWarrantyMonths([FromQuery] int minMonths, [FromQuery] int maxMonths)
        {
            if (minMonths < 0 || maxMonths < 0 || minMonths > maxMonths)
            {
                return BadRequest("Invalid warranty months range");
            }
            var parts = await _partsService.GetPartsByWarrantyMonthsRange(minMonths, maxMonths);
            return Ok(parts);
        }

        [HttpPost("create")]
        [Authorize(Roles = "1, 4")] 
        public async Task<IActionResult> CreatePart([FromBody] CreatePartDto dto)
        {
            try
            {
                var part = await _partsService.CreatePartAsync(dto);
                return CreatedAtAction(nameof(GetPartById), new { partId = part.PartId }, part);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPut("update/{partId}")]
        [Authorize(Roles = "1, 4")]
        public async Task<IActionResult> UpdatePart(int partId, [FromBody] UpdatePartDto dto)
        {
            try
            {
                var updatedPart = await _partsService.UpdatePartAsync(partId, dto);
                return Ok(updatedPart);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpDelete("delete-part/{partId}")]
        [Authorize(Roles = "1,4")]
        public async Task<IActionResult> DeleteService(int partId)
        {
            try
            {
                await _partsService.DeletePartAsync(partId);
                return Ok(new { message = "Part deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("revenue")]
        /*[Authorize(Roles = "1")]*/
        public async Task<IActionResult> GetRevenueReport(int month, int year)
        {
            try
            {
                var report = await _partsService.GenerateRevenueReport(month, year);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("dashboard-summary")]
        /*[Authorize(Roles = "1")]*/
        public async Task<IActionResult> GetDashboardSummary(int month, int year)
        {
            try
            {
                var summary = await _partsService.GenerateDashboardSummary(month, year);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<bool> PartExists(int id)
        {
            var part = await _partsService.GetPartByIdAsync(id);
            return part != null;
        }
    }
}
