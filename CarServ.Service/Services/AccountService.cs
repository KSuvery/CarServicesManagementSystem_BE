using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accRepository;

        public AccountService(IAccountRepository accRepository)
        {
            _accRepository = accRepository;
        }
        public async Task<Users> GetAccountById(int Id)
        {
            return await _accRepository.GetAccountById(Id);
        }

        public async Task<Users> GetAccountByMail(string mail)
        {
            return await _accRepository.GetAccountByMail(mail);
        }

        public async Task<List<Users>> GetAccountByRole(int roleID)
        {
            return await _accRepository.GetAccountByRole(roleID);
        }

        public async Task<List<GetAllUserDTO>> GetAllAccount()
        {
            return await _accRepository.GetAllAccount();
        }

        public async Task<PaginationResult<List<GetAllUserDTO>>> GetAllAccountWithPaging(int currentPage, int pageSize)
        {
            return await _accRepository.GetAllAccountWithPaging(currentPage, pageSize);
        }

        public async Task<Users> Login(string username, string password)
        {
            return await _accRepository.Login(username, password);
        }
        public async Task<Users> SignupNewCustomer(string fullName, string email, string phoneNumber, string password)
        {
           return await _accRepository.SignupNewCustomer(fullName, email, phoneNumber, password);
        }
    }
}
