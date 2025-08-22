using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services
{
    public class Notificationervice : INotificationervice
    {
        private readonly INotificationRepository _notificationRepository;

        public Notificationervice(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<Notification>> GetAllNotificationsAsync()
        {
            return await _notificationRepository.GetAllNotificationsAsync();
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            return await _notificationRepository.DeleteNotificationAsync(notificationId);
        }

        public async Task<Notification> GetNotificationByIdAsync(int notificationId)
        {
            return await _notificationRepository.GetNotificationByIdAsync(notificationId);
        }

        public async Task<List<Notification>> GetNotificationByUserIdAsync(int userId)
        {
            return await _notificationRepository.GetNotificationByUserIdAsync(userId);
        }

        public async Task<Notification> CreateNotificationAsync(NotificationDTO dto)
        {
            return await _notificationRepository.CreateNotificationAsync(dto);
        }

        public async Task<Notification> UpdateNotificationAsync(int notificationId, NotificationDTO dto)
        {
            return await _notificationRepository.UpdateNotificationAsync(notificationId, dto);
        }

        public async Task<Notification> MarkNotificationAsReadAsync(int notificationId, bool isRead)
        {
            return await _notificationRepository.MarkNotificationAsReadAsync(notificationId, isRead);
        }
    }
}
