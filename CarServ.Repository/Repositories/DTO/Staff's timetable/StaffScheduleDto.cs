using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Staff_s_timetable
{
    public class StaffScheduleDto
    {
        public int DayOfWeek { get; set; }  
        public string DayName { get; set; } = string.Empty; 
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan Duration => EndTime - StartTime;  
    }

}
