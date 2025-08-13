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

namespace CarServ.service.WorkerService
{
    public class AdminSeederService
    {
        private readonly CarServicesManagementSystemContext _context;
        private readonly AdminSettings _adminSettings;
        private readonly AdminSettings _customerSettings;
        public AdminSeederService(CarServicesManagementSystemContext context, IConfiguration configuration)
        {
            _context = context;
            _adminSettings = configuration.GetSection("AdminCredentials").Get<AdminSettings>();
            _customerSettings = configuration.GetSection("CustomerCredentials").Get<AdminSettings>();
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
                    adminRole = new Domain.Entities.Role { RoleName = "Admin" };
                    _context.Roles.Add(adminRole);
                    await _context.SaveChangesAsync();
                }
                var adminUser = new User
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
        public async Task SeedCustomerAsync()
        {
            // Check if customer already exists
            var customerExists = await _context.Users.AnyAsync(u => u.Email == _customerSettings.Email);

            if (!customerExists)
            {
                var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Customer");
                if (customerRole == null)
                {
                    customerRole = new Domain.Entities.Role { RoleName = "Customer" };
                    _context.Roles.Add(customerRole);
                    await _context.SaveChangesAsync();
                }
                var customerUser = new User
                {
                    FullName = _customerSettings.Username,
                    Email = _customerSettings.Email,
                    PhoneNumber = _customerSettings.PhoneNumber,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(_customerSettings.Password),
                    RoleId = customerRole.RoleId
                };
                _context.Users.Add(customerUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}
