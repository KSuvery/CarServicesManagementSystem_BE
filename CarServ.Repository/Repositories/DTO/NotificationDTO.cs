using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO
{
    public class NotificationDTO
    {
        public int userId { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public DateTime? sentAt { get; set; }
        public bool isRead { get; set; }
        public string type { get; set; }
    }
}
