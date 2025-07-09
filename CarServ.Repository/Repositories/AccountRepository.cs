using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using CarServ.Repository.Repositories.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace CarServ.Repository.Repositories
{
    public class AccountRepository : GenericRepository<Users>, IAccountRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public AccountRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> DisableAccount(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Users> GetAccountById(int Id)
        {
            var userListTmp = await _context.Users.Include(m => m.Role).FirstOrDefaultAsync(m => m.UserId == Id);
            return userListTmp ?? new Users();
        }

        public async Task<Users> GetAccountByMail(string mail)
        {
            var userListTmp = await _context.Users.Include(m => m.Role).FirstOrDefaultAsync(m => m.Email == mail);
            return userListTmp ?? new Users();
        }

        public async Task<List<Users>> GetAccountByRole(int roleID)
        {
            var userListTmp = await _context.Users
                .Include(m => m.Role)
            .Where(m =>m.RoleId == roleID).ToListAsync();
            return userListTmp ?? new List<Users>();
        }

        public async Task<List<GetAllUserDTO>> GetAllAccount()
        {
            var userListTmp = await _context.Users
                            .Include(u => u.Role)
                            .Select(u => new GetAllUserDTO
                            {
                                UserID = u.UserId,
                                FullName = u.FullName,
                                Email = u.Email,
                                PhoneNumber = u.PhoneNumber,
                                RoleName = u.Role.RoleName // Only include the role name
                            })
                            .ToListAsync();
            return userListTmp ?? new List<GetAllUserDTO>();
        }

            public async Task<PaginationResult<List<GetAllUserDTO>>> GetAllAccountWithPaging(int currentPage, int pageSize)
        {
            var userListTmp = await this.GetAllAccount();

            var totalItems = userListTmp.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            userListTmp = userListTmp.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationResult<List<GetAllUserDTO>>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                Items = userListTmp
            };
        }
    }
}
