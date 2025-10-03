using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.DTO.Staff_s_timetable
{
    public static class DayOfWeekExtensions
    {
        public static string ToVietnamese(this DayOfWeekViet day)
        {
            return day switch
            {
                DayOfWeekViet.Sunday => "Chủ nhật",
                DayOfWeekViet.Monday => "Thứ hai",
                DayOfWeekViet.Tuesday => "Thứ ba",
                DayOfWeekViet.Wednesday => "Thứ tư",
                DayOfWeekViet.Thursday => "Thứ năm",
                DayOfWeekViet.Friday => "Thứ sáu",
                DayOfWeekViet.Saturday => "Thứ bảy",
                _ => ""
            };
        }
    }
}
