using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarTypesController : ControllerBase
    {
        private readonly ICarTypesService _carTypesService;
        public CarTypesController(ICarTypesService carTypesService)
        {
            _carTypesService = carTypesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCarTypes()
        {
            try
            {
                var carTypes = await _carTypesService.GetAllCarTypesAsync();
                return Ok(carTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarTypeById(int id)
        {
            try
            {
                var carType = await _carTypesService.GetCarTypeByIdAsync(id);
                if (carType == null)
                {
                    return NotFound($"Car type with ID {id} not found.");
                }
                return Ok(carType);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search-by-name")]
        public async Task<IActionResult> SearchCarTypes([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }
            try
            {
                var carTypes = await _carTypesService.GetCarTypesByNameAsync(query);
                return Ok(carTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<CarType>> CreateCarTypes(
            string typeName,
            string description)
        {
            var carType = await _carTypesService.AddCarTypeAsync(typeName, description);
            if (carType == null)
            {
                return BadRequest("Failed to create car type.");
            }
            return CreatedAtAction(nameof(GetCarTypeById), new { id = carType.CarTypeId }, carType);
        }

        [HttpPut("update")]
        public async Task<ActionResult<CarType>> UpdateCarType(
            int carTypeId,
            string typeName,
            string description)
        {
            var updatedCarType = await _carTypesService.UpdateCarTypeAsync(carTypeId, typeName, description);
            if(!await CarTypeExists(carTypeId))
            {
                return NotFound($"Car type with ID {carTypeId} not found.");
            }
            return Ok(updatedCarType);
        }

        private async Task<bool> CarTypeExists(int carTypeId)
        {
            var carType = await _carTypesService.GetCarTypeByIdAsync(carTypeId);
            return carType != null;
        }
    }
}
