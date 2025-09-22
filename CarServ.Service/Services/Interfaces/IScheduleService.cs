using CarServ.Repository.Repositories.DTO.Staff_s_timetable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services.Interfaces
{
    public interface IScheduleService
    {
        Task<List<StaffScheduleDto>> GetStaffScheduleAsync(int staffId);
        Task<int> CreateDayOffRequestAsync(int staffId, CreateDayOffRequestDto dto);
        Task<List<DayOffRequestDto>> GetAllDayOffRequestsAsync(int page = 1, int size = 10);
        Task UpdateDayOffRequestStatusAsync(int requestId, string adminEmail, UpdateDayOffRequestDto dto);
    }
}
