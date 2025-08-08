using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notifications> CreateNotificationAsync(
            int userId,
            string title,
            string message,
            DateTime? sentAt,
            bool isRead = false)
        {
            sentAt ??= DateTime.UtcNow;
            return await _notificationRepository.CreateNotificationAsync(userId, title, message, sentAt, isRead);
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            return await _notificationRepository.DeleteNotificationAsync(notificationId);
        }

        public async Task<Notifications> GetNotificationByIdAsync(int notificationId)
        {
            return await _notificationRepository.GetNotificationByIdAsync(notificationId);
        }

        public async Task<List<Notifications>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _notificationRepository.GetNotificationsByUserIdAsync(userId);
        }

        public async Task<Notifications> MarkNotificationAsReadAsync(int notificationId, bool isRead)
        {
            return await _notificationRepository.MarkNotificationAsReadAsync(notificationId, isRead);
        }
    }
}
