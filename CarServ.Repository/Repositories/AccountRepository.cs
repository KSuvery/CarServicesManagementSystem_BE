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
using CarServ.Repository.Repositories.DTO.User_return_DTO;
using System.Net;

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

        public async Task<Users> Login(string username, string password)
        {
            
            // await _context.Users
            //    .FirstOrDefaultAsync(x => x.Phone == username && x.Password == password);

            // await _context.Users
            //    .FirstOrDefaultAsync(x => x.id == username && x.Password == password);

            // await _context.Users
            //    .FirstOrDefaultAsync(x => x.username == username && x.Password == password && x.IsActive);
            
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == username);
            
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        public async Task<CustomerDTO> SignupNewCustomer(string fullName, string email, string phoneNumber, string password, string address)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email))
            {
                throw new Exception("Email already exists.");
            }
            var passwordHash = HashPassword(password);
            var newUser = new Users
            {
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = passwordHash,
                RoleId = 4 // New user is a customer
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            var customer = new Customers
            {   
                CustomerId = newUser.UserId,
                Address = address
            };
            
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            var newlyCreatedCustomer = await this.GetAccountById(customer.CustomerId);
            var userDTO = new CustomerDTO
            {
                UserID = newlyCreatedCustomer.UserId,
                FullName = newlyCreatedCustomer.FullName,
                Email = newlyCreatedCustomer.Email,
                PhoneNumber = newlyCreatedCustomer.PhoneNumber,
                Address = newlyCreatedCustomer.Customers.Address,
                RoleName = newlyCreatedCustomer.Role.RoleName
            };
            return userDTO;
        }


        public async Task<StaffDTO> AddingNewStaff(string fullName, string email, string phoneNumber, string password)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email))
            {
                throw new Exception("Email already exists.");
            }
            var passwordHash = HashPassword(password);
            var newUser = new Users
            {
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = passwordHash,
                RoleId = 2 // New user is a service staff
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            var staff = new ServiceStaff
            {
                StaffId = newUser.UserId
                
            };

            _context.ServiceStaff.Add(staff);
            await _context.SaveChangesAsync();

            var newlyCreatedStaff = await this.GetAccountById(staff.StaffId);
            var staffDTO = new StaffDTO
            {
                UserID = newlyCreatedStaff.UserId,
                FullName = newlyCreatedStaff.FullName,
                Email = newlyCreatedStaff.Email,
                PhoneNumber = newlyCreatedStaff.PhoneNumber,                
                RoleName = newlyCreatedStaff.Role.RoleName
            };
            return staffDTO;
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
