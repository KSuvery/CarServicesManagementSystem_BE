using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using CarServ.Repository.Repositories.DTO.Booking_A_Service;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentervices _Appointmentervices;

        public AppointmentController(IAppointmentervices Appointmentervices)
        {
            _Appointmentervices = Appointmentervices;
        }

        // GET: api/Appointment
        [HttpGet]
        //[Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointment()
        {
            return await _Appointmentervices.GetAllAppointmentAsync();
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "1")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
        {
            var appointment = await _Appointmentervices.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return appointment;
        }

        [HttpGet("GetByCustomerId/{customerId}")]
        //[Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentByCustomerId(int customerId)
        {
            var Appointment = await _Appointmentervices.GetAppointmentByCustomerIdAsync(customerId);
            if (Appointment == null || !Appointment.Any())
            {
                return NotFound();
            }
            return Appointment;
        }

        [HttpGet("GetByVehicleId/{vehicleId}")]
        //[Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentByVehicleId(int vehicleId)
        {
            var Appointment = await _Appointmentervices.GetAppointmentByVehicleIdAsync(vehicleId);
            if (Appointment == null || !Appointment.Any())
            {
                return NotFound();
            }
            return Appointment;
        }

        [HttpPost("schedule")]
        //[Authorize(Roles = "1,2,3")]
        public async Task<IActionResult> ScheduleAppointment(int customerId, [FromBody] ScheduleAppointmentDto dto)
        {
            try
            {
                var appointment = await _Appointmentervices.ScheduleAppointment(customerId, dto);
                return CreatedAtAction(nameof(ScheduleAppointment), new { id = appointment.AppointmentId }, appointment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{appointmentId}")]
        /*[Authorize(Roles = "1,2")]*/
        public async Task<ActionResult<Appointment>> UpdateAppointment(int appointmentId, string status)
        {
            if (!await AppointmentExists(appointmentId))
            {
                return NotFound();
            }
            var updatedAppointment = await _Appointmentervices.UpdateAppointmentAsync(appointmentId, status);
            if (updatedAppointment == null)
            {
                return BadRequest("Unable to update appointment.");
            }
            return Ok(updatedAppointment);
        }

        private async Task<bool> AppointmentExists(int id)
        {
            return await _Appointmentervices.GetAppointmentByIdAsync(id) != null;
        }
    }
}
