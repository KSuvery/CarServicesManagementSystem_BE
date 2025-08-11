using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.service.Services.Interfaces;
using CarServ.Repository.Repositories.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {        
        private readonly IAccountService _accService;

        public AccountController( IAccountService accService)
        {          
            _accService = accService;
        }        



        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<PaginationResult<List<GetAllUserDTO>>> Get(int currentPage = 1, int pageSize = 5)
        {
            return await _accService.GetAllAccountWithPaging(currentPage, pageSize);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<User>> GetAccountById(int id)
        {
            var acc = await _accService.GetAccountById(id);
            if (acc == null)
            {
                return NotFound();
            }
            return acc;
        }

        [HttpGet("{mail}")]
        [Authorize(Roles = "1")]
        public async Task<ActionResult<User>> GetAccountByMail(string mail)
        {
            var acc = await _accService.GetAccountByMail(mail);
            if (acc == null)
            {
                return NotFound();
            }
            return acc;
        }

        [HttpGet("{roleID}")]
        [Authorize(Roles = "1")]
        public async Task<List<User>> GetAccountByRole(int roleID)
        {
            return await _accService.GetAccountByRole(roleID);
        }


    }
}
