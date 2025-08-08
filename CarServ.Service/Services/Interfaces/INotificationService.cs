using CarServ.Service.Services.Interfaces;
using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> GetNotificationByIdAsync(int notificationId);
        Task<List<Notification>> GetNotificationsByUserIdAsync(int userId);
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
