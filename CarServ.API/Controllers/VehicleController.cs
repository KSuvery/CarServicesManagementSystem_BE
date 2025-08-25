using CarServ.Repository.Repositories.DTO;
using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
                if (vehicle == null)
                {
                    return NotFound($"Vehicle with ID {id} not found.");
                }
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("search-make")]
        public async Task<IActionResult> SearchVehicles([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty.");
            }
            try
            {
                var vehicles = await _vehicleService.GetVehiclesByMakeAsync(query);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("license-plate/{licensePlate}")]
        public async Task<IActionResult> GetVehicleByLicensePlate(string licensePlate)
        {
            if (string.IsNullOrWhiteSpace(licensePlate))
            {
                return BadRequest("License plate cannot be empty.");
            }
            try
            {
                var vehicle = await _vehicleService.GetVehicleByLicensePlateAsync(licensePlate);
                if (vehicle == null)
                {
                    return NotFound($"Vehicle with license plate {licensePlate} not found.");
                }
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("model/{model}")]
        public async Task<IActionResult> GetVehiclesByModel(string model)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                return BadRequest("Model cannot be empty.");
            }
            try
            {
                var vehicles = await _vehicleService.GetVehiclesByModelAsync(model);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("year-range")]
        public async Task<IActionResult> GetVehiclesByYearRange([FromQuery] int minYear, [FromQuery] int maxYear)
        {
            if (minYear <= 0 || maxYear <= 0 || minYear > maxYear)
            {
                return BadRequest("Invalid year range.");
            }
            try
            {
                var vehicles = await _vehicleService.GetVehiclesByYearRangeAsync(minYear, maxYear);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetVehiclesByCustomerId(int customerId)
        {
            if (customerId <= 0)
            {
                return BadRequest("Invalid customer ID.");
            }
            try
            {
                var vehicles = await _vehicleService.GetVehiclesByCustomerIdAsync(customerId);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("car-type/{carTypeId}")]
        public async Task<IActionResult> GetVehiclesByCarTypeId(int carTypeId)
        {
            if (carTypeId <= 0)
            {
                return BadRequest("Invalid car type ID.");
            }
            try
            {
                var vehicles = await _vehicleService.GetVehiclesByCarTypeIdAsync(carTypeId);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("booked-vehicles")]
        public async Task<IActionResult> GetBookedVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetBookedVehiclesAsync();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("available-vehicles")]
        public async Task<IActionResult> GetAvailableVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetAvailableVehiclesAsync();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("in-service-vehicles")]
        public async Task<IActionResult> GetInServiceVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetInServiceVehiclesAsync();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddVehicle(int customerId, [FromBody] AddVehicleDto dto)
        {
            try
            {
                var vehicle = await _vehicleService.AddVehicleAsync(customerId, dto);
                return CreatedAtAction(nameof(AddVehicle), new { id = vehicle.VehicleId }, vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove/{id}")]
        public async Task<IActionResult> RemoveVehicle(int id)
        {
            try
            {
                var result = await _vehicleService.RemoveVehicleAsync(id);
                if (!result)
                {
                    return NotFound($"Vehicle with ID {id} not found.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
