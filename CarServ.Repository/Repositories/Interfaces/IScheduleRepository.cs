using CarServ.Repository.Repositories.DTO.Staff_s_timetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IScheduleRepository
    {
        Task<List<StaffScheduleDto>> GetStaffScheduleAsync(int staffId);
        Task<int> CreateDayOffRequestAsync(int staffId, CreateDayOffRequestDto dto);
        Task<List<DayOffRequestDto>> GetAllDayOffRequestsAsync(int page = 1, int size = 10);
        Task UpdateDayOffRequestStatusAsync(int requestId, string adminEmail, UpdateDayOffRequestDto dto);
        Task DeleteStaffScheduleAsync(int staffId, int dayOfWeek);
        Task<WeeklyStaffScheduleDto> CreateOrUpdateWeeklyStaffScheduleAsync(int staffId, CreateWeeklyStaffScheduleDto dto);
        Task<SystemWeeklyScheduleDto> GetSystemWeeklyScheduleAsync(DateOnly? weekStart = null, TimeOnly businessStart = default, TimeOnly businessEnd = default);
    }
}
