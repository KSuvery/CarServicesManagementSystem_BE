using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Logging_part_usage
{
    public class UpdateServiceProgressDto
    {
        public int AppointmentId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
    }

}
