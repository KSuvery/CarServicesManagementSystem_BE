using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Booking_A_Service
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string VehicleLicensePlate { get; set; }
        public string VehicleMake { get; set; }
        public string VehicleModel { get; set; }
        public List<string> services { get; set; }
        public int Duration { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string Status { get; set; }
    }

}
