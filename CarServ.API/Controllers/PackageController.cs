using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;
using CarServ.Repository.Repositories.DTO.Service_._ServicePackage;
using CarServ.Repository.Repositories.DTO.Service_managing;
using CarServ.service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarServ.API.Controllers
{
    [ApiController]
    [Route("api/services")]
    [Authorize]
    public class PackageController : ControllerBase
    {
        private readonly IPackageServices _service;

        public PackageController(IPackageServices service)
        {
            _service = service;
        }

        [HttpPost("create-service")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> CreateService([FromBody] CreateServiceDto dto)
        {
            try
            {
                var service = await _service.CreateService(dto);
                return CreatedAtAction(nameof(CreateService), new { id = service.ServiceId }, service);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("create-service-package")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> CreateServicePackage([FromBody] CreateServicePackageDto dto)
        {
            try
            {
                var servicePackage = await _service.CreateServicePackage(dto);
                return CreatedAtAction(nameof(CreateServicePackage), new { id = servicePackage.PackageId }, servicePackage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-service-packages")]
        [Authorize(Roles = "1,2,4")]
        public async Task<PaginationResult<List<ServicePackageDto>>> GetAllServicePackages(int currentPage = 1, int pageSize = 5)
        {   
                return await _service.GetAllServicePackageWithPaging(currentPage, pageSize);           
        }

        [HttpGet("get-all-services")]
        [Authorize(Roles = "1,2,4")]
        public async Task<PaginationResult<List<ServiceDto>>> GetAllServices(int currentPage = 1, int pageSize = 5)
        {
                return await _service.GetAllServicesWithPaging(currentPage, pageSize);
        }        

        [HttpGet("GetAllAvailableVehicleWithCustomerId/{id}")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetAllAvailableVehicleWithCustomerId(int id)
        {
            try
            {
                var vehicles = await _service.GetVehiclesByCustomerId(id);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetAllPartsForSingleService/{serviceid}")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetAllPartsForSingleService(int serviceid)
        {
            try
            {
                var vehicles = await _service.GetPartsByServiceId(serviceid);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("GetAllPartsForPackageService/{packageid}")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetAllPartsForPackageService(int packageid)
        {
            try
            {
                var vehicles = await _service.GetPartsByPackageId(packageid);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("update-service/{serviceId}")]
        [Authorize(Roles = "1,4")] 
        public async Task<IActionResult> UpdateService(int serviceId, [FromBody] UpdateServiceDto dto)
        {
            try
            {
                var updatedService = await _service.UpdateServiceAsync(serviceId, dto);
                return Ok(updatedService);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-service-package/{packageId}")]
        [Authorize(Roles = "1,4")] 
        public async Task<IActionResult> UpdateServicePackage(int packageId, [FromBody] UpdateServicePackageDto dto)
        {
            try
            {
                var updatedPackage = await _service.UpdateServicePackageAsync(packageId, dto);
                return Ok(updatedPackage);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-service/{serviceId}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteService(int serviceId)
        {
            try
            {
                await _service.DeleteServiceAsync(serviceId);
                return Ok(new { message = "Service deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete-service-package/{packageId}")]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteServicePackage(int packageId)
        {
            try
            {
                await _service.DeleteServicePackageAsync(packageId);
                return Ok(new { message = "Service package deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-service-package/{packageId}")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<IActionResult> GetServicePackage(int packageId)
        {
            try
            {
                var package = await _service.GetServicePackage(packageId);
                return Ok(package);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("get-service/{serviceId}")]
        [Authorize(Roles = "1,2,4")]
        public async Task<IActionResult> GetService(int serviceId)
        {
            try
            {
                var serviceDto = await _service.GetService(serviceId);
                return Ok(serviceDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("generate-daily-services-revenue-report")]
        [Authorize(Roles = "1,3,4")]
        public async Task<IActionResult> GenerateDailyServicesRevenueReport(DateTime date)
        {
            try
            {
                var report = await _service.GenerateDailyServicesRevenueReport(date);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}
