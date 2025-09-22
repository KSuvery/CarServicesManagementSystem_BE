using CarServ.Repository.Repositories.DTO.Staff_s_timetable;
using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService services)
        {
            _scheduleService = services;
        }
        [HttpGet("{staffId}/working-schedule")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<List<StaffScheduleDto>>> GetStaffSchedule(int staffId)
        {
            try
            {
                var schedule = await _scheduleService.GetStaffScheduleAsync(staffId);
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
