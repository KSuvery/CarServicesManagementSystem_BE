using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.service.Services.Interfaces;
using CarServ.service.Services;
using CarServ.Repository.Repositories.DTO.Logging_part_usage;
using Microsoft.AspNetCore.Authorization;
using CarServ.Service.Services.Interfaces;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PartController : ControllerBase
    {
        private readonly IPartsService _PartServices;

        public PartController(IPartsService PartServices)
        {
            _PartServices = PartServices;
        }

        [HttpGet]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<IEnumerable<PartDto>>> GetPartItems()
        {
            return await _PartServices.GetAllPartsAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<PartDto>> GetPartItemById(int id)
        {
            var PartItem = await _PartServices.GetPartByIdAsync(id);
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
            var PartItems = await _PartServices.GetPartsByPartName(partName);
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
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths)
        {
            var newPartItem = await _PartServices.AddPartAsync(
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
            int quantity,
            decimal unitPrice,
            DateOnly expiryDate,
            int warrantyMonths)
        {
            if (!await PartExists(id))
            {
                return NotFound();
            }
            var updatedPartItem = await _PartServices.UpdatePartAsync(
                id, partName, quantity, unitPrice, expiryDate, warrantyMonths);
            if (updatedPartItem == null)
            {
                return BadRequest("Failed to update Part item.");
            }
            return Ok(updatedPartItem);
        }

        private async Task<bool> PartExists(int id)
        {
            return await _PartServices.GetPartByIdAsync(id) != null;
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
        [HttpPut("updateServiceProgress")]
        [Authorize(Roles = "1,3,4")]
        public async Task<IActionResult> UpdateServiceProgress([FromBody] UpdateServiceProgressDto dto)
        {
            try
            {
                await _PartServices.UpdateServiceProgress(dto);
                return NoContent(); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
