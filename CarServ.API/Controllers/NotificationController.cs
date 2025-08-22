using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.service.Services.Interfaces;
using CarServ.Repository.Repositories.DTO;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationervice _Notificationervice;

        public NotificationController(INotificationervice Notificationervice)
        {
            _Notificationervice = Notificationervice;
        }

        [HttpGet]
        public async Task<ActionResult<List<Notification>>> GetAllNotifications()
        {
            var notifications = await _Notificationervice.GetAllNotificationsAsync();
            if (notifications == null || !notifications.Any())
            {
                return NotFound();
            }
            return notifications;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotificationById(int id)
        {
            var notification = await _Notificationervice.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return notification;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Notification>>> GetNotificationByUserId(int userId)
        {
            var Notification = await _Notificationervice.GetNotificationByUserIdAsync(userId);
            if (Notification == null || !Notification.Any())
            {
                return NotFound();
            }
            return Notification;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            if (!await NotificationExists(id))
            {
                return NotFound();
            }
            var deleted = await _Notificationervice.DeleteNotificationAsync(id);
            if (!deleted)
            {
                return BadRequest("Failed to delete notification.");
            }
            return NoContent();
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Notification>> CreateNotification([FromBody] NotificationDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Notification data is required.");
            }
            var createdNotification = await _Notificationervice.CreateNotificationAsync(dto);
            if (createdNotification == null)
            {
                return BadRequest("Failed to create notification.");
            }
            return CreatedAtAction(nameof(GetNotificationById), new { id = createdNotification.NotificationId }, createdNotification);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] NotificationDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid notification data.");
            }
            if (!await NotificationExists(id))
            {
                return NotFound();
            }
            var updatedNotification = await _Notificationervice.UpdateNotificationAsync(id, dto);
            if (updatedNotification == null)
            {
                return BadRequest("Failed to update notification.");
            }
            return NoContent();
        }

        [HttpPut("MarkAsRead/{id}")]
        public async Task<IActionResult> MarkNotificationAsRead(int id, bool isRead)
        {
            if (!await NotificationExists(id))
            {
                return NotFound();
            }
            var readNotification = await _Notificationervice.MarkNotificationAsReadAsync(id, isRead);
            if (readNotification == null)
            {
                return BadRequest("Failed to mark notification as read.");
            }
            return NoContent();
        }


        private async Task<bool> NotificationExists(int id)
        {
            return await _Notificationervice.GetNotificationByIdAsync(id) != null;
        }
    }
}
