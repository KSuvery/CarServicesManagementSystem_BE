using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.WorkerService
{
    public class AdminSeederService
    {
        private readonly CarServicesManagementSystemContext _context;
        private readonly AdminSettings _adminSettings;
        public AdminSeederService(CarServicesManagementSystemContext context, IConfiguration configuration)
        {
            _context = context;
            _adminSettings = configuration.GetSection("AdminCredentials").Get<AdminSettings>();
        }

        public async Task SeedAdminAsync()
        {
            // Check if admin already exists
            var adminExists = await _context.Users.AnyAsync(u => u.Email == _adminSettings.Email);

            if (!adminExists)
            {
                var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
                if (adminRole == null)
                {
                    adminRole = new Roles { RoleName = "Admin" };
                    _context.Roles.Add(adminRole);
                    await _context.SaveChangesAsync();
                }
                var adminUser = new Users
                {
                    FullName = _adminSettings.Username,
                    Email = _adminSettings.Email,
                    PhoneNumber = _adminSettings.PhoneNumber,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(_adminSettings.Password),
                    RoleId = adminRole.RoleId
                };
                _context.Users.Add(adminUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}
