using CarServ.Repository.Repositories.DTO.Staff_s_timetable;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository sRepository)
        {
            _scheduleRepository = sRepository;
        }

        public async Task<int> CreateDayOffRequestAsync(int staffId, CreateDayOffRequestDto dto)
        {
            return await _scheduleRepository.CreateDayOffRequestAsync(staffId, dto);
        }

        public async Task<WeeklyStaffScheduleDto> CreateOrUpdateWeeklyStaffScheduleAsync(int staffId, CreateWeeklyStaffScheduleDto dto)
        {
            return await _scheduleRepository.CreateOrUpdateWeeklyStaffScheduleAsync(staffId, dto);
        }

        public async Task DeleteStaffScheduleAsync(int staffId, int dayOfWeek)
        {
             await _scheduleRepository.DeleteStaffScheduleAsync(staffId, dayOfWeek);
        }

        public async Task<List<DayOffRequestDto>> GetAllDayOffRequestsAsync(int page = 1, int size = 10)
        {
            return await _scheduleRepository.GetAllDayOffRequestsAsync(page, size);
        }

        public async Task<List<StaffScheduleDto>> GetStaffScheduleAsync(int staffId)
        {
            return await _scheduleRepository.GetStaffScheduleAsync(staffId);
        }

        public async Task<SystemWeeklyScheduleDto> GetSystemWeeklyScheduleAsync(DateOnly? weekStart = null, TimeOnly businessStart = default, TimeOnly businessEnd = default)
        {
            return await _scheduleRepository.GetSystemWeeklyScheduleAsync(weekStart, businessStart, businessEnd);
        }

        public async Task UpdateDayOffRequestStatusAsync(int requestId, string adminEmail, UpdateDayOffRequestDto dto)
        {
            await _scheduleRepository.UpdateDayOffRequestStatusAsync(requestId, adminEmail, dto);
        }
    }
}
