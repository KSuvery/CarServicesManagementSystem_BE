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
        [HttpGet("working-schedule/{staffId}")]
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
        [HttpPost("dayoff/make-request/{staffId}")]
        [Authorize(Roles = "3")]  
        public async Task<ActionResult<int>> CreateDayOffRequest(int staffId, [FromBody] CreateDayOffRequestDto dto)
        {
            try
            {                
                var requestId = await _scheduleService.CreateDayOffRequestAsync(staffId, dto);
                return Ok(new { RequestId = requestId, Message = "Request submitted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("dayoff/view-requests")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<List<DayOffRequestDto>>> GetAllDayOffRequests([FromQuery] int page = 1,
                                                                                     [FromQuery] int size = 10)
        {
            var requests = await _scheduleService.GetAllDayOffRequestsAsync(page, size);
            return Ok(requests);
        }

        [HttpPut("dayoff/{requestId}/status")]
        [Authorize(Roles = "1")]
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

        [HttpPost("weekly-schedule/{staffId}")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<WeeklyStaffScheduleDto>> CreateOrUpdateWeeklyStaffSchedule(int staffId, [FromBody] CreateWeeklyStaffScheduleDto dto)
        {
            try
            {                
                var result = await _scheduleService.CreateOrUpdateWeeklyStaffScheduleAsync(staffId, dto);
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
        private string GetCurrentUser()
        {
            if (User?.Identity == null || !User.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated");

            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            return email;
        }
    }
}
