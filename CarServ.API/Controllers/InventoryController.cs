using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;
using CarServ.Service.Services;
using CarServ.Repository.Repositories.DTO.Logging_part_usage;
using Microsoft.AspNetCore.Authorization;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PartController : ControllerBase
    {
        private readonly IPartServices _PartServices;

        public PartController(IPartServices PartServices)
        {
            _PartServices = PartServices;
        }

        [HttpGet]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<IEnumerable<Part>>> GetPartItems()
        {
            return await _PartServices.GetAllPartItemsAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<Part>> GetPartItemById(int id)
        {
            var PartItem = await _PartServices.GetPartItemByIdAsync(id);
            if (PartItem == null)
            {
                return NotFound();
            }
            return PartItem;
        }

        [HttpGet("GetByName/{partName}")]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<IEnumerable<Part>>> GetPartItemsByName(string partName)
        {
            var PartItems = await _PartServices.GetPartItemsByNameAsync(partName);
            if (PartItems == null || !PartItems.Any())
            {
                return NotFound();
            }
            return PartItems;
        }

        [HttpPost("Create")]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<Part>> CreatePartItem(
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            var newPartItem = await _PartServices.CreatePartItemAsync(
                partName, quantity, unitPrice, expiryDate, warrantyMonths);
            if (newPartItem == null)
            {
                return BadRequest("Failed to create Part item.");
            }

            return CreatedAtAction(nameof(GetPartItemById), new { id = newPartItem.PartId }, newPartItem);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<Part>> UpdatePartItem(
            int id,
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            if (!await PartExists(id))
            {
                return NotFound();
            }
            var updatedPartItem = await _PartServices.UpdatePartItemAsync(
                id, partName, quantity, unitPrice, expiryDate, warrantyMonths);
            if (updatedPartItem == null)
            {
                return BadRequest("Failed to update Part item.");
            }
            return Ok(updatedPartItem);
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "1,3")]
        public async Task<IActionResult> RemovePartItem(int id)
        {
            if (!await PartExists(id))
            {
                return NotFound();
            }
            var result = await _PartServices.RemovePartItemAsync(id);
            if (!result)
            {
                return BadRequest("Failed to delete Part item.");
            }
            return NoContent();
        }

        private async Task<bool> PartExists(int id)
        {
            return await _PartServices.GetPartItemByIdAsync(id) != null;
        }

        [HttpGet("revenueReport")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> GetRevenueReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
        {
            try
            {
                var report = await _PartServices.GenerateRevenueReport(startDate, endDate);
                return Ok(report);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Something is wrong while generating the report");
            }
        }

        [HttpPost("track-parts-used")]
        [Authorize(Roles = "1,2,3")]
        public IActionResult TrackPartsUsed([FromBody] PartUsageDto partsUsedDTO)
        {
            if (partsUsedDTO == null)
            {
                return BadRequest("Invalid data.");
            }
            _PartServices.TrackPartsUsed(partsUsedDTO);
            return Ok("Parts used added successfully.");
        }
    }
}
