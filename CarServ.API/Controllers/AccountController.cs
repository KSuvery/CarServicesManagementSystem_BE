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
using CarServ.Repository.Repositories.DTO.User_return_DTO;

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

        [HttpPut("update-profile/{userId}")]
        [Authorize(Roles = "2")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateProfileDto dto)
        {
            try
            {
                var updatedUser = await _accService.UpdateProfileAsync(userId, dto);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public async Task<PaginationResult<List<GetAllUserDTO>>> Get(int currentPage = 1, int pageSize = 5)
        {
            return await _accService.GetAllAccountWithPaging(currentPage, pageSize);
        }

        [HttpGet("by-id/{id}")]
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

        [HttpGet("by-mail/{mail}")]
        [Authorize(Roles = "1,2,3,4")]
        public async Task<ActionResult<User>> GetAccountByMail(string mail)
        {
            var acc = await _accService.GetAccountByMail(mail);
            if (acc == null)
            {
                return NotFound();
            }
            return acc;
        }

        [HttpGet("by-role/{roleID}")]
        [Authorize(Roles = "1")]
        public async Task<List<User>> GetAccountByRole(int roleID)
        {
            return await _accService.GetAccountByRole(roleID);
        }


    }
}
