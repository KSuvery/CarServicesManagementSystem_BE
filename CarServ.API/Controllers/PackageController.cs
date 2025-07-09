using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarServ.API.Controllers
{
    [ApiController]
    [Route("api/services")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageServices _service;

        public PackageController(IPackageServices service)
        {
            _service = service;
        }

        [HttpGet]
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
    }

}
