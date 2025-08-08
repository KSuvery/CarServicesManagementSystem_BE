using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Booking_A_Service
{
    public class ScheduleAppointmentDto
    {
        public int? VehicleId { get; set; }
        public int? PackageId { get; set; }
        public List<int> ServiceIds { get; set; } = new List<int>();
        public int? PromotionId { get; set; }
        public DateTime AppointmentDate { get; set; }
    }

}
