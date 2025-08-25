using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.User_return_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Repository.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<User> UpdateProfileAsync(int userId, UpdateProfileDto dto);
        Task<List<GetAllUserDTO>> GetAllAccount();
        Task<PaginationResult<List<GetAllUserDTO>>> GetAllAccountWithPaging(int currentPage, int pageSize);
        Task<User> GetAccountById(int Id);
        Task<CustomerWithVehiclesDTO> GetAccountByMail(string mail);
        Task<List<User>> GetAccountByRole(int roleID);

        Task<User> UpdateAccountStatusAsync(int userId, bool status);
        Task<User> Login(string username, string password);
        Task<User> SignupNewCustomer(string fullName, string email, string phoneNumber, string password, string address);
        Task<StaffDTO> AddingNewStaff(string fullName, string email, string phoneNumber, string password, string address, int roleID);
        Task<List<ServiceStaff>> GetAllServiceStaff();
        Task<ServiceStaff> GetServiceStaffById(int id);
        Task<List<InventoryManager>> GetAllInventoryManagers();
        Task<InventoryManager> GetInventoryManagerById(int id);
        Task<List<Customer>> GetAllCustomers();
    }
}
