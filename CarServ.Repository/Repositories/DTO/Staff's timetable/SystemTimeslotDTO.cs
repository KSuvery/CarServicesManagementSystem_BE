using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Staff_s_timetable
{
    public class SystemTimeSlotDto
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }   
        public List<StaffAvailabilityDto> AvailableStaff { get; set; } = new List<StaffAvailabilityDto>();  // Staff in this slot
        public int SlotDuration => (EndTime - StartTime).Hours;  
    }
    public class StaffAvailabilityDto
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty; 
        public string? Specialty { get; set; }  
        public decimal? Rating { get; set; }          
    }

    public class SystemWeeklyScheduleDto
    {
        public DateOnly WeekStart { get; set; }  
        public DateOnly WeekEnd { get; set; }   
        public Dictionary<int, DayScheduleDto> Days { get; set; } = new Dictionary<int, DayScheduleDto>();  
    }

    public class DayScheduleDto
    {
        public int DayOfWeek { get; set; }
        public string DayName { get; set; } = string.Empty;  
        public List<SystemTimeSlotDto> TimeSlots { get; set; } = new List<SystemTimeSlotDto>();
        public int TotalAvailableStaff { get; set; }  
    }

    public enum DayOfWeekViet
    {
        Sunday = 7,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

}
