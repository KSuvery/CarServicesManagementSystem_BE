using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartsController : ControllerBase
    {
        private readonly IPartsService _partsService;
        public PartsController(IPartsService partsService)
        {
            _partsService = partsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Part>>> GetAllParts()
        {
            var parts = await _partsService.GetAllPartsAsync();
            return Ok(parts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Part>> GetPartById(int id)
        {
            var part = await _partsService.GetPartByIdAsync(id);
            if (part == null)
            {
                return NotFound();
            }
            return Ok(part);
        }

        [HttpGet("search")]
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
        public async Task<ActionResult<Part>> CreatePart(
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths)
        {
            var createdPart = await _partsService.AddPartAsync(partName, quantity, unitPrice, expiryDate, warrantyMonths);

            if (createdPart == null)
            {
                return BadRequest("Part cannot be null");
            }

            return CreatedAtAction(nameof(GetPartById), new { id = createdPart.PartId }, createdPart);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Part>> UpdatePart(
            int partId,
            string partName,
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths)
        {
            if (!await PartExists(partId))
            {
                return NotFound();
            }
            var updatedPart = await _partsService.UpdatePartAsync(partId, partName, quantity, unitPrice, expiryDate, warrantyMonths);
            if (updatedPart == null)
            {
                return BadRequest("Part cannot be null");
            }
            return Ok(updatedPart);
        }

        private async Task<bool> PartExists(int id)
        {
            var part = await _partsService.GetPartByIdAsync(id);
            return part != null;
        }
    }
}
