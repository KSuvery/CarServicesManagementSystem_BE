using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Staff_s_timetable;
using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpGet("working-schedule/{userId}")]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<List<StaffScheduleDto>>> GetStaffSchedule(int userId)
        {
            try
            {
                var schedule = await _scheduleService.GetStaffScheduleAsync(userId);
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("dayoff/make-request/{userId}")]
        [Authorize(Roles = "1,3")]  
        public async Task<ActionResult<int>> CreateDayOffRequest(int userId, [FromBody] CreateDayOffRequestDto dto)
        {
            try
            {                
                var requestId = await _scheduleService.CreateDayOffRequestAsync(userId, dto);
                return Ok(new { RequestId = requestId, Message = "Request submitted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("dayoff/view-requests")]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<List<DayOffRequestDto>>> GetAllDayOffRequests([FromQuery] int page = 1,
                                                                                     [FromQuery] int size = 10)
        {
            var requests = await _scheduleService.GetAllDayOffRequestsAsync(page, size);
            return Ok(requests);
        }

        [HttpPut("dayoff/{requestId}/status")]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult> UpdateDayOffRequestStatus(int requestId, [FromBody] UpdateDayOffRequestDto dto)
        {
            try
            {
                var email = GetCurrentUser();  // From auth
                await _scheduleService.UpdateDayOffRequestStatusAsync(requestId, email, dto);
                return Ok(new { Message = $"Request {dto.Status.ToLower()} successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("weekly-schedule/{userId}")]
        [Authorize(Roles = "1,3")]
        public async Task<ActionResult<WeeklyStaffScheduleDto>> CreateOrUpdateWeeklyStaffSchedule(int userId, [FromBody] CreateWeeklyStaffScheduleDto dto)
        {
            try
            {                
                var result = await _scheduleService.CreateOrUpdateWeeklyStaffScheduleAsync(userId, dto);
                return Ok(new
                {
                    Message = $"Weekly schedule updated successfully. {result.UpdatedDays} days modified.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("system-schedule")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<SystemWeeklyScheduleDto>> GetSystemWeeklySchedule(
        [FromQuery] DateOnly? startDate = null,
        [FromQuery] TimeOnly? businessHoursStart = null,
        [FromQuery] TimeOnly? businessHoursEnd = null)
        {
            try
            {
                var schedule = await _scheduleService.GetSystemWeeklyScheduleAsync(startDate, businessHoursStart ?? default, businessHoursEnd ?? default);
                return Ok(new { Message = "System weekly timetable retrieved successfully.", Data = schedule });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
        }
        private string GetCurrentUser()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return email;
        }
    }
}
