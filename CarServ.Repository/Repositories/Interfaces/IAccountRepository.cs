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
        Task<List<GetAllUserDTO>> GetAllAccount();
        Task<PaginationResult<List<GetAllUserDTO>>> GetAllAccountWithPaging(int currentPage, int pageSize);
        Task<User> GetAccountById(int Id);
        Task<User> GetAccountByMail(string mail);
        Task<List<User>> GetAccountByRole(int roleID);

        Task<bool> DisableAccount(int Id);
        Task<User> Login(string username, string password);
        Task<CustomerDTO> SignupNewCustomer(string fullName, string email, string phoneNumber, string password, string address);
        Task<StaffDTO> AddingNewStaff(string fullName, string email, string phoneNumber, string password);
    }
}
