using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryServices _inventoryServices;

        public InventoryController(IInventoryServices inventoryServices)
        {
            _inventoryServices = inventoryServices;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventoryItems()
        {
            return await _inventoryServices.GetAllInventoryItemsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventoryItemById(int id)
        {
            var inventoryItem = await _inventoryServices.GetInventoryItemByIdAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }
            return inventoryItem;
        }

        [HttpGet("GetByName/{partName}")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventoryItemsByName(string partName)
        {
            var inventoryItems = await _inventoryServices.GetInventoryItemsByNameAsync(partName);
            if (inventoryItems == null || !inventoryItems.Any())
            {
                return NotFound();
            }
            return inventoryItems;
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Inventory>> CreateInventoryItem(
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            var newInventoryItem = await _inventoryServices.CreateInventoryItemAsync(
                partName, quantity, unitPrice, expiryDate, warrantyMonths);
            if (newInventoryItem == null)
            {
                return BadRequest("Failed to create inventory item.");
            }

            return CreatedAtAction(nameof(GetInventoryItemById), new { id = newInventoryItem.PartId }, newInventoryItem);
        }

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<Inventory>> UpdateInventoryItem(
            int id,
            string partName,
            int? quantity,
            decimal? unitPrice,
            DateOnly? expiryDate,
            int? warrantyMonths)
        {
            if (!await InventoryExists(id))
            {
                return NotFound();
            }
            var updatedInventoryItem = await _inventoryServices.UpdateInventoryItemAsync(
                id, partName, quantity, unitPrice, expiryDate, warrantyMonths);
            if (updatedInventoryItem == null)
            {
                return BadRequest("Failed to update inventory item.");
            }
            return Ok(updatedInventoryItem);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> RemoveInventoryItem(int id)
        {
            if (!await InventoryExists(id))
            {
                return NotFound();
            }
            var result = await _inventoryServices.RemoveInventoryItemAsync(id);
            if (!result)
            {
                return BadRequest("Failed to delete inventory item.");
            }
            return NoContent();
        }

        private async Task<bool> InventoryExists(int id)
        {
            return await _inventoryServices.GetInventoryItemByIdAsync(id) != null;
        }

        [HttpGet("revenueReport")]
        public async Task<IActionResult> GetRevenueReport(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
        {
            try
            {
                var report = await _inventoryServices.GenerateRevenueReport(startDate, endDate);
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
    }
}
