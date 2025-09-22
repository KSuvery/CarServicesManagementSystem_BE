using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Staff_s_timetable
{
    public class CreateDayOffRequestDto
    {
        public DateOnly RequestedDate { get; set; } 
        public string Reason { get; set; } = string.Empty;  
    }

    public class DayOffRequestDto
    {
        public int RequestId { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; } = string.Empty; 
        public DateOnly RequestedDate { get; set; }
        public string DayOfWeek { get; set; } = string.Empty;  
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime RequestedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string? AdminName { get; set; } 
        public string? AdminNotes { get; set; }
    }

    public class UpdateDayOffRequestDto
    {
        public string Status { get; set; } = "Approved";  
        public string? AdminNotes { get; set; }  
    }


}
