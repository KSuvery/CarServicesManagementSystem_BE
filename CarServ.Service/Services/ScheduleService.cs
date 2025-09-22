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

        public async Task<List<StaffScheduleDto>> GetStaffScheduleAsync(int staffId)
        {
            return await _scheduleRepository.GetStaffScheduleAsync(staffId);
        }
    }
}
