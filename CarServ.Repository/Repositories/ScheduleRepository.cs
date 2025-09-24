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
            var staff = await _context.ServiceStaffs
                        .FirstOrDefaultAsync(ss => ss.UserId == staffId);
            if (staff == null)
            {
                throw new Exception($"No staff record found for UserId {staffId}.");
            }
            var schedules = await _context.StaffSchedules
            .Where(s => s.StaffId == staff.StaffId && s.IsActive)
            .OrderBy(s => s.DayOfWeek)
            .Select(s => new StaffScheduleDto
            {
                ScheduleId = s.ScheduleId,
                DayOfWeek = s.DayOfWeek,
                DayName = ((DayOfWeek)s.DayOfWeek).ToString(),
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsActive = s.IsActive,
                UpdatedAt = s.UpdatedAt
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
            var staff = await _context.ServiceStaffs
                        .FirstOrDefaultAsync(ss => ss.UserId == staffId);
            if (staff == null)
            {
                throw new Exception($"No staff record found for UserId {staffId}.");
            }
            var existing = await _context.DayOffRequests
                .AnyAsync(r => r.StaffId == staff.StaffId && r.RequestedDate == dto.RequestedDate && r.Status == "Pending");
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

        public async Task<StaffScheduleDto> CreateOrUpdateStaffScheduleAsync(int staffId, CreateStaffScheduleDto dto)
        {

            var staff = await _context.ServiceStaffs
                        .FirstOrDefaultAsync(ss => ss.UserId == staffId);            
            if (staff == null)
            {
                throw new Exception($"Staff with ID {staffId} not found.");
            }

            // Validate DayOfWeek (1-7)
            if (dto.DayOfWeek < 1 || dto.DayOfWeek > 7)
            {
                throw new ArgumentException("DayOfWeek must be between 1 (Monday) and 7 (Sunday).");
            }

            // Validate times: End > Start
            if (dto.EndTime <= dto.StartTime)
            {
                throw new ArgumentException("End time must be after start time.");
            }
            var existingSchedule = await _context.StaffSchedules
                .FirstOrDefaultAsync(s => s.StaffId == staff.StaffId && s.DayOfWeek == dto.DayOfWeek);

            StaffSchedule schedule;
            if (existingSchedule != null)
            {
                existingSchedule.StartTime = dto.StartTime;
                existingSchedule.EndTime = dto.EndTime;
                existingSchedule.IsActive = dto.IsActive;
                existingSchedule.UpdatedAt = DateTime.Now;
                schedule = existingSchedule;
            }
            else
            {
                schedule = new StaffSchedule
                {
                    StaffId = staff.StaffId,
                    DayOfWeek = (byte)dto.DayOfWeek,
                    StartTime = dto.StartTime,
                    EndTime = dto.EndTime,
                    IsActive = dto.IsActive,
                    UpdatedAt = DateTime.Now
                };
                _context.StaffSchedules.Add(schedule);
            }

            await _context.SaveChangesAsync();


            return new StaffScheduleDto
            {
                ScheduleId = schedule.ScheduleId,
                DayOfWeek = schedule.DayOfWeek,
                DayName = ((DayOfWeek)schedule.DayOfWeek).ToString(),
                StartTime = schedule.StartTime,
                EndTime = schedule.EndTime,
                IsActive = schedule.IsActive,
                UpdatedAt = schedule.UpdatedAt
            };
        }

        public async Task<WeeklyStaffScheduleDto> CreateOrUpdateWeeklyStaffScheduleAsync(int staffId, CreateWeeklyStaffScheduleDto dto)
        {
            var staff = await _context.ServiceStaffs
                        .FirstOrDefaultAsync(ss => ss.UserId == staffId);            
            if (staff == null)
            {
                throw new Exception($"Staff with ID {staffId} not found.");
            }

            if (dto.DailySchedules == null || !dto.DailySchedules.Any())
            {
                throw new ArgumentException("At least one daily schedule must be provided.");
            }

            var dayGroups = dto.DailySchedules.GroupBy(s => s.DayOfWeek);
            if (dayGroups.Any(g => g.Count() > 1))
            {
                throw new ArgumentException("Duplicate DayOfWeek entries found. Each day can only be specified once.");
            }

            var invalidDays = new List<int>();
            foreach (var daily in dto.DailySchedules)
            {
                if (daily.DayOfWeek < 1 || daily.DayOfWeek > 7)
                {
                    invalidDays.Add(daily.DayOfWeek);
                }
                else if (daily.EndTime <= daily.StartTime)
                {
                    throw new ArgumentException($"Invalid times for DayOfWeek {daily.DayOfWeek}: End time must be after start time.");
                }
            }
            if (invalidDays.Any())
            {
                throw new ArgumentException($"Invalid DayOfWeek values: {string.Join(", ", invalidDays)}. Must be 1-7.");
            }
            var updatedSchedules = new List<StaffScheduleDto>();
            var updatedCount = 0;
            using var transaction = await _context.Database.BeginTransactionAsync();  
            try
            {
                foreach (var dailyDto in dto.DailySchedules)
                {
                    var singleDto = new CreateStaffScheduleDto
                    {
                        DayOfWeek = dailyDto.DayOfWeek,
                        StartTime = dailyDto.StartTime,
                        EndTime = dailyDto.EndTime,
                        IsActive = dailyDto.IsActive,
                        Notes = dailyDto.Notes
                    };
                    var updatedSchedule = await CreateOrUpdateStaffScheduleAsync(staffId, singleDto);  
                    updatedSchedules.Add(updatedSchedule);
                    updatedCount++;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                var fullWeek = await GetStaffScheduleAsync(staffId); 
                return new WeeklyStaffScheduleDto
                {
                    Schedules = fullWeek,  
                    UpdatedDays = updatedCount
                };
            }
            catch
            {
                await transaction.RollbackAsync();
                //Throw lại để debug
                throw;  
            }
        }


        public async Task DeleteStaffScheduleAsync(int staffId, int dayOfWeek)
        {
            var staff = await _context.ServiceStaffs
                        .FirstOrDefaultAsync(ss => ss.UserId == staffId);
            if (staff == null)
            {
                throw new Exception($"No staff record found for UserId {staffId}.");
            }
            var schedule = await _context.StaffSchedules
                .FirstOrDefaultAsync(s => s.StaffId == staff.StaffId && s.DayOfWeek == dayOfWeek);

            if (schedule == null)
            {
                throw new Exception($"No schedule found for staff {staffId} on day {dayOfWeek}.");
            }
            schedule.IsActive = false;
            schedule.UpdatedAt = DateTime.Now;
            // _context.StaffSchedules.Remove(schedule);

            await _context.SaveChangesAsync();
        }

        public async Task<SystemWeeklyScheduleDto> GetSystemWeeklyScheduleAsync(DateOnly? weekStart = null, TimeOnly businessStart = default, TimeOnly businessEnd = default)
        {
            weekStart ??= DateOnly.FromDateTime(DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1));  
            var weekEnd = DateOnly.FromDateTime(DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1)).AddDays(6); 

            businessStart = businessStart == default ? new TimeOnly(8, 0) : businessStart;
            businessEnd = businessEnd == default ? new TimeOnly(18, 0) : businessEnd;

            var allSchedules = await _context.StaffSchedules
                .Include(s => s.Staff).ThenInclude(ss => ss.User)  
                .Where(s => s.IsActive)
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToListAsync();

            var daysByDayOfWeek = allSchedules.GroupBy(s => s.DayOfWeek).ToDictionary(g => g.Key, g => g.ToList());

            var slots = GenerateHourlySlots(businessStart, businessEnd);

            var response = new SystemWeeklyScheduleDto
            {
                WeekStart = (DateOnly)weekStart,
                WeekEnd = weekEnd,
                Days = new Dictionary<int, DayScheduleDto>()
            };

            for (int day = 1; day <= 7; day++)
            {
                List<StaffSchedule> daySchedules;
                if (!daysByDayOfWeek.TryGetValue((byte)day, out daySchedules))
                {
                    daySchedules = new List<StaffSchedule>(); 
                }
                var dayDto = new DayScheduleDto
                {
                    DayOfWeek = day,
                    DayName = ((DayOfWeek)day).ToString(),
                    TimeSlots = new List<SystemTimeSlotDto>()
                };

                foreach (var slot in slots)
                {
                    var availableStaff = new List<StaffAvailabilityDto>();
                    var uniqueStaffIds = new HashSet<int>();  

                    foreach (var schedule in daySchedules)
                    {
                        var staff = schedule.Staff;
                        if (staff == null) continue;

                        if (Overlaps(slot.Start, slot.End, schedule.StartTime, schedule.EndTime) &&
                            !uniqueStaffIds.Contains(staff.StaffId))
                        {
                            uniqueStaffIds.Add(staff.StaffId);
                            availableStaff.Add(new StaffAvailabilityDto
                            {
                                StaffId = staff.StaffId,
                                StaffName = staff.User?.FullName ?? "Unknown",
                                Specialty = staff.Specialty,
                                Rating = staff.Rating
                            });
                        }
                    }

                    dayDto.TimeSlots.Add(new SystemTimeSlotDto
                    {
                        StartTime = slot.Item1,
                        EndTime = slot.Item2,
                        AvailableStaff = availableStaff.OrderBy(s => s.StaffName).ToList()  
                    });
                }

                dayDto.TotalAvailableStaff = daySchedules.Select(s => s.StaffId).Distinct().Count();

                response.Days[day] = dayDto;
            }

            return response;
        }

        private List<(TimeOnly Start, TimeOnly End)> GenerateHourlySlots(TimeOnly businessStart, TimeOnly businessEnd)
        {
            var slots = new List<(TimeOnly, TimeOnly)>();
            var current = businessStart;
            while (current < businessEnd)
            {
                var next = current.AddHours(1);
                if (next > businessEnd) next = businessEnd;  
                slots.Add((current, next));
                current = next;
            }
            return slots;
        }
        private bool Overlaps(TimeOnly slotStart, TimeOnly slotEnd, TimeOnly shiftStart, TimeOnly shiftEnd)
        {
            return slotStart < shiftEnd && slotEnd > shiftStart;
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
               
            }            
        }


    }
}
