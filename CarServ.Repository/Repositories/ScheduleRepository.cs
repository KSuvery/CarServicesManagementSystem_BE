using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO.Staff_s_timetable;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories
{
    public class ScheduleRepository : GenericRepository<StaffSchedule>, IScheduleRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public ScheduleRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }
        public async Task<List<StaffScheduleDto>> GetStaffScheduleAsync(int staffId)
        {
            var schedules = await _context.StaffSchedules
                .Where(s => s.StaffId == staffId && s.IsActive)
                .OrderBy(s => s.DayOfWeek)  
                .Select(s => new StaffScheduleDto
                {
                    DayOfWeek = s.DayOfWeek,
                    DayName = ((DayOfWeek)s.DayOfWeek).ToString(),
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsActive = s.IsActive
                })
                .ToListAsync();

            var fullWeek = new List<StaffScheduleDto>();
            for (int day = 1; day <= 7; day++)
            {
                var schedule = schedules.FirstOrDefault(s => s.DayOfWeek == day);
                if (schedule != null)
                {
                    fullWeek.Add(schedule);
                }
                else
                {
                    fullWeek.Add(new StaffScheduleDto
                    {
                        DayOfWeek = day,
                        DayName = ((DayOfWeek)day).ToString(),
                        StartTime = default, 
                        EndTime = default,
                        IsActive = false
                    });
                }
            }

            return fullWeek;
        }

    }
}
