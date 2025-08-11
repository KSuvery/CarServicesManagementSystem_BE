using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentServices _appointmentServices;

        public AppointmentController(IAppointmentServices appointmentServices)
        {
            _appointmentServices = appointmentServices;
        }

        // GET: api/Appointment
        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            return await _appointmentServices.GetAllAppointmentsAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int id)
        {
            var appointment = await _appointmentServices.GetAppointmentByIdAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return appointment;
        }

        [HttpGet("GetByCustomerId/{customerId}")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByCustomerId(int customerId)
        {
            var appointments = await _appointmentServices.GetAppointmentsByCustomerIdAsync(customerId);
            if (appointments == null || !appointments.Any())
            {
                return NotFound();
            }
            return appointments;
        }

        [HttpGet("GetByVehicleId/{vehicleId}")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByVehicleId(int vehicleId)
        {
            var appointments = await _appointmentServices.GetAppointmentsByVehicleIdAsync(vehicleId);
            if (appointments == null || !appointments.Any())
            {
                return NotFound();
            }
            return appointments;
        }

        [HttpPost("Schedule")]
        /*[Authorize(Roles = "1,2")]*/
        public async Task<ActionResult<Appointment>> ScheduleAppointment(
            int customerId,
            int vehicleId,
            int packageId,
            DateTime appointmentDate,
            string status = "Pending",
            int? promotionId = null)
        {
            var createdAppointment = await _appointmentServices.ScheduleAppointmentAsync(
                customerId,
                vehicleId,
                packageId,
                appointmentDate,
                status,
                promotionId);

            if (createdAppointment == null)
            {
                return BadRequest("Unable to schedule appointment.");
            }

            return CreatedAtAction(nameof(GetAppointmentById), new { id = createdAppointment.AppointmentId }, createdAppointment);
        }

        [HttpPut("{appointmentId}")]
        /*[Authorize(Roles = "1,2")]*/
        public async Task<ActionResult<Appointment>> UpdateAppointment(
            int appointmentId, int customerId, int vehicleId, int packageId, DateTime appointmentDate, string status, int? promotionId)
        {
            if (!await AppointmentsExists(appointmentId))
            {
                return NotFound();
            }
            var updatedAppointment = await _appointmentServices.UpdateAppointmentAsync(appointmentId, customerId, vehicleId, packageId, appointmentDate, status, promotionId);
            if (updatedAppointment == null)
            {
                return BadRequest("Unable to update appointment.");
            }
            return Ok(updatedAppointment);
        }

        private async Task<bool> AppointmentsExists(int id)
        {
            return await _appointmentServices.GetAppointmentByIdAsync(id) != null;
        }
    }
}
