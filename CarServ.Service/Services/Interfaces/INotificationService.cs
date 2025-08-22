using CarServ.service.Services.Interfaces;
using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarServ.Repository.Repositories.DTO;

namespace CarServ.service.Services.Interfaces
{
    public interface INotificationervice
    {
        Task<List<Notification>> GetAllNotificationsAsync();
        Task<Notification> GetNotificationByIdAsync(int notificationId);
        Task<List<Notification>> GetNotificationByUserIdAsync(int userId);
        Task<Notification> CreateNotificationAsync(NotificationDTO dto);
        Task<Notification> UpdateNotificationAsync(
            int notificationId,
            NotificationDTO dto);
        Task<Notification> MarkNotificationAsReadAsync(
            int notificationId,
            bool isRead);
        Task<bool> DeleteNotificationAsync(int notificationId);
    }
}
