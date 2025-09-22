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
    }
}
