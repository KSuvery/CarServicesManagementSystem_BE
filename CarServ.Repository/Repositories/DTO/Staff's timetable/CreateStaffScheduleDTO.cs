using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Staff_s_timetable
{
    public class CreateStaffScheduleDto
    {
        public int DayOfWeek { get; set; } 
        public TimeOnly StartTime { get; set; } 
        public TimeOnly EndTime { get; set; }   
        public bool IsActive { get; set; } = true;  
        public string? Notes { get; set; }  
    }

    public class CreateWeeklyStaffScheduleDto
    {
        public List<CreateStaffScheduleDto> DailySchedules { get; set; } = new List<CreateStaffScheduleDto>();  
    }
}
