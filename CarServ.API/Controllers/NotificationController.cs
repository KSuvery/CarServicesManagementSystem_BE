using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Notifications>> GetNotificationById(int id)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            return notification;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Notifications>>> GetNotificationsByUserId(int userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            if (notifications == null || !notifications.Any())
            {
                return NotFound();
            }
            return notifications;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            if (!await NotificationsExists(id))
            {
                return NotFound();
            }
            var deleted = await _notificationService.DeleteNotificationAsync(id);
            if (!deleted)
            {
                return BadRequest("Failed to delete notification.");
            }
            return NoContent();
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Notifications>> CreateNotification(
            int userId,
            string message,
            DateTime? notificationDate = null,
            bool isRead = false)
        {
            notificationDate ??= DateTime.UtcNow;
            var newNotification = await _notificationService.CreateNotificationAsync(
                userId, message, notificationDate, isRead);
            if (newNotification == null)
            {
                return BadRequest("Failed to create notification.");
            }
            return CreatedAtAction(nameof(GetNotificationById), new { id = newNotification.NotificationId }, newNotification);
        }

        [HttpPut("MarkAsRead/{id}")]
        public async Task<IActionResult> MarkNotificationAsRead(int id, bool isRead)
        {
            if (!await NotificationsExists(id))
            {
                return NotFound();
            }
            var readNotification = await _notificationService.MarkNotificationAsReadAsync(id, isRead);
            if (readNotification == null)
            {
                return BadRequest("Failed to mark notification as read.");
            }
            return NoContent();
        }


        private async Task<bool> NotificationsExists(int id)
        {
            return await _notificationService.GetNotificationByIdAsync(id) != null;
        }
    }
}
