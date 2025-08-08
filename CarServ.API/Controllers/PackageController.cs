using CarServ.Service.Services.Interfaces;
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
    }

}
