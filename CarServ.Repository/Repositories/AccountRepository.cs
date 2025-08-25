using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.User_return_DTO;
using CarServ.Repository.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarServ.Repository.Repositories
{
    public class AccountRepository : GenericRepository<User>, IAccountRepository
    {
        private readonly CarServicesManagementSystemContext _context;
        public AccountRepository(CarServicesManagementSystemContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            
            var user = await _context.Users
                .Include(u => u.UserId) 
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                throw new Exception("User  not found.");
            }
            
            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.Address = dto.Address;
            
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email && u.UserId != userId))
            {
                throw new Exception("Email already exists.");
            }
            
            await _context.SaveChangesAsync();

            return user; 
        }

        public async Task<User> UpdateAccountStatusAsync(int userId, bool status)
        {
            
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            
            user.IsActive = status;

            
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> GetAccountById(int Id)
        {
            var userListTmp = await _context.Users.Include(m => m.Role).FirstOrDefaultAsync(m => m.UserId == Id);
            return userListTmp ?? new User();
        }

        public async Task<CustomerWithVehiclesDTO> GetAccountByMail(string mail)
        {
            var customer = await _context.Users.Include(m => m.Role).
                                    FirstOrDefaultAsync(m => m.Email == mail);                        
            if(customer != null)
            {
                var listVehicles = await _context.Vehicles
                    .Where(v => v.Customer.UserId == customer.UserId)
                    .ToListAsync();
                var userDTO = new CustomerWithVehiclesDTO
                {
                    UserID = customer.UserId,
                    FullName = customer.FullName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    RoleName = customer.Role.RoleName,
                    Address = customer.Address,
                    vehicles = listVehicles

                };
                return userDTO ?? new CustomerWithVehiclesDTO();
            }
            throw new Exception("Cannot find user with mail: " + mail);

        }

        public async Task<List<User>> GetAccountByRole(int roleID)
        {
            var userListTmp = await _context.Users
                .Include(m => m.Role)
            .Where(m => m.RoleId == roleID).ToListAsync();
            return userListTmp ?? new List<User>();
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
                                RoleName = u.Role.RoleName, // Only include the role name
                                Address = u.Address
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

        public async Task<User> Login(string username, string password)
        {

            // await _context.User
            //    .FirstOrDefaultAsync(x => x.Phone == username && x.Password == password);

            // await _context.User
            //    .FirstOrDefaultAsync(x => x.id == username && x.Password == password);

            // await _context.User
            //    .FirstOrDefaultAsync(x => x.username == username && x.Password == password && x.IsActive);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        public async Task<User> SignupNewCustomer(string fullName, string email, string phoneNumber, string password, string address)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email))
            {
                throw new Exception("Email already exists.");
            }
            var passwordHash = HashPassword(password);
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = passwordHash,
                Address = address,
                RoleId = 2
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            var customer = new Customer
            {
                UserId = newUser.UserId,
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return newUser;

            //var newlyCreatedCustomer = await this.GetAccountById(customer.CustomerId);
            //var userDTO = new CustomerDTO
            //{
            //    UserID = newlyCreatedCustomer.UserId,
            //};
            //return userDTO;
        }


        public async Task<StaffDTO> AddingNewStaff(string fullName, string email, string phoneNumber, string password, string address, int roleID)
        {
            if (await _context.Users.AnyAsync(x => x.Email == email))
            {
                throw new Exception("Email already exists.");
            }
            var passwordHash = HashPassword(password);
            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = passwordHash,
                Address = address,
                RoleId = roleID
            };
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            if(roleID == 3)
            {
                var staff = new ServiceStaff
                {
                    UserId = newUser.UserId
                };

                _context.ServiceStaffs.Add(staff);
                await _context.SaveChangesAsync();
            }
            else if(roleID == 4)
            {
                var inventoryManager = new InventoryManager
                {
                    UserId = newUser.UserId
                };

                _context.InventoryManagers.Add(inventoryManager);
                await _context.SaveChangesAsync();
            }

            var newlyCreatedStaff = await this.GetAccountById(newUser.UserId);
            var staffDTO = new StaffDTO
            {
                FullName = newlyCreatedStaff.FullName,
                Email = newlyCreatedStaff.Email,
                PhoneNumber = newlyCreatedStaff.PhoneNumber,
                Address = newlyCreatedStaff.Address,
                RoleName = newlyCreatedStaff.Role.RoleName
            };
            
            return staffDTO;
        }

        public async Task<List<ServiceStaff>> GetAllServiceStaff()
        {
            return await _context.ServiceStaffs
                .Include(s => s.User) 
                .ToListAsync();
        }

        public async Task<ServiceStaff> GetServiceStaffById(int id)
        {
            return await _context.ServiceStaffs
                .FirstOrDefaultAsync(s => s.StaffId == id);
        }

        public async Task<List<InventoryManager>> GetAllInventoryManagers()
        {
            return await _context.InventoryManagers
                .Include(s => s.User) 
                .ToListAsync();
        }

        public async Task<InventoryManager> GetInventoryManagerById(int id)
        {
            return await _context.InventoryManagers
                .FirstOrDefaultAsync(s => s.ManagerId == id);
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _context.Customers
                .Include(c => c.User) 
                .ToListAsync();
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }

}
