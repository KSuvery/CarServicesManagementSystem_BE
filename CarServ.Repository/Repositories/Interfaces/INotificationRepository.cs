using CarServ.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface INotificationRepository : IGenericRepository<Notifications>
    {
        Task<Notifications> GetNotificationByIdAsync(int notificationId);
        Task<List<Notifications>> GetNotificationsByUserIdAsync(int userId);
        Task<Notifications> CreateNotificationAsync(
            int userId,
            string message,
            DateTime? sentAt = null,
            bool isRead = false);
        Task<Notifications> MarkNotificationAsReadAsync(
            int notificationId,
            bool isRead);
        Task<bool> DeleteNotificationAsync(int notificationId);
    }
}
