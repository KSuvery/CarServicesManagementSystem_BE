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

        public async Task<int> CreateDayOffRequestAsync(int staffId, CreateDayOffRequestDto dto)
        {      
            var existing = await _context.DayOffRequests
                .AnyAsync(r => r.StaffId == staffId && r.RequestedDate == dto.RequestedDate && r.Status == "Pending");
            if (existing)
            {
                throw new InvalidOperationException("A pending request already exists for this date.");
            }

            if (dto.RequestedDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new ArgumentException("Requested date must be today or in the future.");
            }
            var request = new DayOffRequest
            {
                StaffId = staffId,
                RequestedDate = dto.RequestedDate,
                Reason = dto.Reason,
                Status = "Pending",
                RequestedAt = DateTime.Now
            };

            _context.DayOffRequests.Add(request);
            await _context.SaveChangesAsync();

            return request.RequestId;  
        }

        public async Task<List<DayOffRequestDto>> GetAllDayOffRequestsAsync(int page = 1, int size = 10)
        {
            var query = _context.DayOffRequests
                .Include(r => r.Staff).ThenInclude(s => s.User)  
                .Include(r => r.ApprovedByUser)  
                .AsQueryable();

            query = query.OrderByDescending(r => r.RequestedAt).Skip((page - 1) * size).Take(size);

            var requests = await query.ToListAsync();

            return requests.Select(r => new DayOffRequestDto
            {
                RequestId = r.RequestId,
                StaffId = r.StaffId,
                StaffName = r.Staff.User.FullName,
                RequestedDate = r.RequestedDate,
                DayOfWeek = r.RequestedDate.DayOfWeek.ToString(),  
                Reason = r.Reason,
                Status = r.Status,
                RequestedAt = r.RequestedAt,
                ApprovedAt = r.ApprovedAt,
                AdminName = r.ApprovedByUser?.FullName,
                AdminNotes = r.AdminNotes
            }).ToList();
        }

        public async Task UpdateDayOffRequestStatusAsync(int requestId, string adminEmail, UpdateDayOffRequestDto dto)
        {
            var request = await _context.DayOffRequests
                .Include(r => r.Staff)
                    .ThenInclude(s => s.StaffSchedules)  
                .FirstOrDefaultAsync(r => r.RequestId == requestId);
            User? admin = null;
            if (adminEmail != null) admin = await _context.Users.FirstOrDefaultAsync(c => c.Email == adminEmail);
            if (request == null)
            {
                throw new Exception("Request not found.");
            }

            if (request.Status != "Pending")
            {
                throw new InvalidOperationException("Only pending requests can be updated.");
            }

            if (dto.Status != "Approved" && dto.Status != "Rejected")
            {
                throw new ArgumentException("Status must be 'Approved' or 'Rejected'.");
            }

            // Update request
            request.Status = dto.Status;
            request.ApprovedAt = DateTime.Now;
            request.ApprovedByUser  = admin;
            request.AdminNotes = dto.AdminNotes;

            if (dto.Status == "Approved")
            {
                await AdjustScheduleForDayOffAsync(request.StaffId, request.RequestedDate);
            }

            await _context.SaveChangesAsync();
        }

        private async Task AdjustScheduleForDayOffAsync(int staffId, DateOnly requestedDate)
        {
            var dayOfWeek = (int)requestedDate.DayOfWeek;  

            // (DayOfWeek.Sunday=0, but our enum is 1-7 with 7=Sunday)
            if (requestedDate.DayOfWeek == DayOfWeek.Sunday)
            {
                dayOfWeek = 7;
            }
            else
            {
                dayOfWeek = (int)requestedDate.DayOfWeek;
            }

            var schedule = await _context.StaffSchedules
                .FirstOrDefaultAsync(s => s.StaffId == staffId && s.DayOfWeek == dayOfWeek);

            if (schedule != null)
            {
                schedule.IsActive = false;  // Toggle off
                schedule.UpdatedAt = DateTime.Now;
                // Optional: Add a Note field to StaffSchedule for reason (e.g., "Day off approved on [date]")
            }            
        }


    }
}
