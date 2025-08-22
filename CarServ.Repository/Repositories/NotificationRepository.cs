using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories
{
    public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
    {
        private readonly CarServicesManagementSystemContext _context;

        public NotificationRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .Include(n => n.User)
                .ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int notificationId)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        public async Task<List<Notification>> GetNotificationByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }

        public async Task<Notification> CreateNotificationAsync(NotificationDTO dto)
        {
            var notification = new Notification
            {
                UserId = dto.userId,
                Title = dto.title,
                Message = dto.message,
                SentAt = dto.sentAt ?? DateTime.Now,
                IsRead = false,
                Type = dto.type
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> UpdateNotificationAsync(
            int notificationId,
            NotificationDTO dto)
        {
            var notification = await GetNotificationByIdAsync(notificationId);
            if (notification == null)
            {
                return null;
            }
            notification.Title = dto.title;
            notification.Message = dto.message;
            notification.SentAt = dto.sentAt ?? DateTime.Now;
            notification.IsRead = dto.isRead;
            notification.Type = dto.type;
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await GetNotificationByIdAsync(notificationId);
            if (notification == null)
            {
                return false;
            }
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Notification> MarkNotificationAsReadAsync(
            int notificationId,
            bool isRead)
        {
            var notification = await GetNotificationByIdAsync(notificationId);
            if (notification != null)
            {
                isRead = true;
                notification.IsRead = isRead;
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();
            }
            return notification;
        }
    }
}
