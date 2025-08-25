using CarServ.Domain.Entities;
using CarServ.Repository.Repositories.DTO;
using CarServ.Repository.Repositories.DTO.User_return_DTO;
using CarServ.Repository.Repositories.Interfaces;
using CarServ.service.Services.Interfaces;
using CarServ.Service.Services;
using CarServ.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarServ.service.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accRepository;
        private readonly IEmailService _emailService;

        public AccountService(IAccountRepository accRepository, IEmailService emailService)
        {
            _accRepository = accRepository;
            _emailService = emailService;
        }
        public async Task<User> GetAccountById(int Id)
        {
            return await _accRepository.GetAccountById(Id);
        }

        public async Task<CustomerWithVehiclesDTO> GetAccountByMail(string mail)
        {
            return await _accRepository.GetAccountByMail(mail);
        }

        public async Task<List<User>> GetAccountByRole(int roleID)
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

        public async Task<User> Login(string username, string password)
        {
            return await _accRepository.Login(username, password);
        }
        public async Task<User> SignupNewCustomer(string fullName, string email, string phoneNumber, string password, string address)
        {
           var user = await _accRepository.SignupNewCustomer(fullName, email, phoneNumber, password, address);
            await _emailService.SendEmailAsync(
            email,
            "Signing up new account confirmation",
            "<h1>Thank you!</h1><p>you have successfuly registered a new account with the following detail: </p>"
        );
            return user;
        }
        public async Task<StaffDTO> AddingNewStaff(string fullName, string email, string phoneNumber, string password, string address, int roleID)
        {
            return await _accRepository.AddingNewStaff(fullName, email, phoneNumber, password, address, roleID);
        }

        public async Task<User> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            return await _accRepository.UpdateProfileAsync(userId, dto);
        }
        public async Task<List<ServiceStaff>> GetAllServiceStaff()
        {
            return await _accRepository.GetAllServiceStaff();
        }

        public async Task<ServiceStaff> GetServiceStaffById(int id)
        {
            return await _accRepository.GetServiceStaffById(id);
        }
        public async Task<User> UpdateAccountStatusAsync(int userId, bool status)
        {
            return await _accRepository.UpdateAccountStatusAsync(userId, status);
        }

        public async Task<List<InventoryManager>> GetAllInventoryManagers()
        {
            return await _accRepository.GetAllInventoryManagers();
        }

        public async Task<InventoryManager> GetInventoryManagerById(int id)
        {
            return await _accRepository.GetInventoryManagerById(id);
        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            return await _accRepository.GetAllCustomers();
        }
    }
}
