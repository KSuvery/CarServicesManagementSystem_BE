using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;
using CarServ.Repository.Repositories.DTO;
using Microsoft.AspNetCore.Authorization;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accService;

        public AccountController(IAccountService accService)
        {
            _accService = accService;
        }

        
        [HttpGet]
        public async Task<PaginationResult<List<GetAllUserDTO>>> Get(int currentPage = 1, int pageSize = 5)
        {
            return await _accService.GetAllAccountWithPaging(currentPage, pageSize);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetAccountById(int id)
        {
            var acc = await _accService.GetAccountById(id);
            if (acc == null)
            {
                return NotFound();
            }
            return acc;
        }

        [HttpGet("{mail}")]
        public async Task<ActionResult<Users>> GetAccountByMail(string mail)
        {
            var acc = await _accService.GetAccountByMail(mail);
            if (acc == null)
            {
                return NotFound();
            }
            return acc;
        }

        [HttpGet("{roleID}")]      
        public async Task<List<Users>> GetAccountByRole(int roleID)
        {
            return await _accService.GetAccountByRole(roleID);
        }


    }
}
