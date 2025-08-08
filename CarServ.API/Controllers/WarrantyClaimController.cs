using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.Service.Services.Interfaces;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarrantyClaimController : ControllerBase
    {
        private readonly IWarrantyClaimService _warrantyClaimService;

        public WarrantyClaimController(IWarrantyClaimService warrantyClaimService)
        {
            _warrantyClaimService = warrantyClaimService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarrantyClaims>>> GetAllWarrantyClaims()
        {
            var warrantyClaims = await _warrantyClaimService.GetAllWarrantyClaimsAsync();
            return Ok(warrantyClaims);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarrantyClaims>> GetWarrantyClaimById(int id)
        {
            var warrantyClaim = await _warrantyClaimService.GetWarrantyClaimByIdAsync(id);
            if (warrantyClaim == null)
            {
                return NotFound();
            }
            return Ok(warrantyClaim);
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<WarrantyClaims>>> GetWarrantyClaimsBySupplierId(int supplierId)
        {
            var warrantyClaims = await _warrantyClaimService.GetWarrantyClaimsBySupplierIdAsync(supplierId);
            if (warrantyClaims == null || !warrantyClaims.Any())
            {
                return NotFound();
            }
            return Ok(warrantyClaims);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<WarrantyClaims>>> GetWarrantyClaimsByStatus(string status)
        {
            var warrantyClaims = await _warrantyClaimService.GetWarrantyClaimsByStatusAsync(status);
            if (warrantyClaims == null || !warrantyClaims.Any())
            {
                return NotFound();
            }
            return Ok(warrantyClaims);
        }

        [HttpGet("claimDate/{claimDate}")]
        public async Task<ActionResult<IEnumerable<WarrantyClaims>>> GetWarrantyClaimsByClaimDate(DateOnly claimDate)
        {
            var warrantyClaims = await _warrantyClaimService.GetWarrantyClaimsByClaimDateAsync(claimDate);
            if (warrantyClaims == null || !warrantyClaims.Any())
            {
                return NotFound();
            }
            return Ok(warrantyClaims);
        }

        [HttpGet("note/{note}")]
        public async Task<ActionResult<IEnumerable<WarrantyClaims>>> GetWarrantyClaimsByNote(string note)
        {
            var warrantyClaims = await _warrantyClaimService.GetWarrantyClaimsByNoteAsync(note);
            if (warrantyClaims == null || !warrantyClaims.Any())
            {
                return NotFound();
            }
            return Ok(warrantyClaims);
        }

        [HttpPost("create")]
        public async Task<ActionResult<WarrantyClaims>> CreateWarrantyClaim(
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes)
        {
            var warrantyClaim = await _warrantyClaimService.CreateWarrantyClaimAsync(partId, supplierId, claimDate, status, notes);
            if (warrantyClaim == null)
            {
                return BadRequest("Failed to create warranty claim.");
            }
            return CreatedAtAction(nameof(GetWarrantyClaimById), new { id = warrantyClaim.ClaimId }, warrantyClaim);
        }

        [HttpPut("update/{claimId}")]
        public async Task<ActionResult<WarrantyClaims>> UpdateWarrantyClaim(
            int claimId,
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes)
        {
            var warrantyClaim = await _warrantyClaimService.UpdateWarrantyClaimAsync(claimId, partId, supplierId, claimDate, status, notes);
            if (warrantyClaim == null)
            {
                return NotFound();
            }
            return Ok(warrantyClaim);
        }

        [HttpDelete("deactivate/{claimId}")]
        public async Task<ActionResult<WarrantyClaims>> DeactivateWarrantyClaim(int claimId)
        {
            var warrantyClaim = await _warrantyClaimService.DeactivateWarrantyClaimAsync(claimId);
            if (warrantyClaim == null)
            {
                return NotFound();
            }
            return Ok(warrantyClaim);
        }
    }
}
