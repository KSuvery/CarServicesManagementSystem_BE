using CarServ.service.Services.Interfaces;
using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services.Interfaces
{
    public interface INotificationervice
    {
        Task<Notification> GetNotificationByIdAsync(int notificationId);
        Task<List<Notification>> GetNotificationByUserIdAsync(int userId);
        Task<Notification> CreateNotificationAsync(
            int userId,            
            string title,
            string message,
            DateTime? sentAt = null,
            bool isRead = false);
        Task<Notification> MarkNotificationAsReadAsync(
            int notificationId,
            bool isRead);
        Task<bool> DeleteNotificationAsync(int notificationId);
    }
}
