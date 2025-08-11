using CarServ.Domain.Entities;
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

        [HttpGet]
        [Authorize(Roles = "1,2,4")]
        public async Task<IActionResult> GetAllServicePackages()
        {
            try
            {
                var packages = await _service.GetAllServicePackages();
                return Ok(packages);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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

    }

}
