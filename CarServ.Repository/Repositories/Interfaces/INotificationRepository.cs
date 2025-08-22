using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notification>
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
