using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarServ.Domain.Entities;
using CarServ.service.Services.Interfaces;

namespace CarServ.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarrantyClaimController : ControllerBase
    {
        private readonly IWarrantyClaimervice _WarrantyClaimervice;

        public WarrantyClaimController(IWarrantyClaimervice WarrantyClaimervice)
        {
            _WarrantyClaimervice = WarrantyClaimervice;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarrantyClaim>>> GetAllWarrantyClaim()
        {
            var WarrantyClaim = await _WarrantyClaimervice.GetAllWarrantyClaimAsync();
            return Ok(WarrantyClaim);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarrantyClaim>> GetWarrantyClaimById(int id)
        {
            var warrantyClaim = await _WarrantyClaimervice.GetWarrantyClaimByIdAsync(id);
            if (warrantyClaim == null)
            {
                return NotFound();
            }
            return Ok(warrantyClaim);
        }

        [HttpGet("supplier/{supplierId}")]
        public async Task<ActionResult<IEnumerable<WarrantyClaim>>> GetWarrantyClaimBySupplierId(int supplierId)
        {
            var WarrantyClaim = await _WarrantyClaimervice.GetWarrantyClaimBySupplierIdAsync(supplierId);
            if (WarrantyClaim == null || !WarrantyClaim.Any())
            {
                return NotFound();
            }
            return Ok(WarrantyClaim);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<WarrantyClaim>>> GetWarrantyClaimByStatus(string status)
        {
            var WarrantyClaim = await _WarrantyClaimervice.GetWarrantyClaimByStatusAsync(status);
            if (WarrantyClaim == null || !WarrantyClaim.Any())
            {
                return NotFound();
            }
            return Ok(WarrantyClaim);
        }

        [HttpGet("claimDate/{claimDate}")]
        public async Task<ActionResult<IEnumerable<WarrantyClaim>>> GetWarrantyClaimByClaimDate(DateOnly claimDate)
        {
            var WarrantyClaim = await _WarrantyClaimervice.GetWarrantyClaimByClaimDateAsync(claimDate);
            if (WarrantyClaim == null || !WarrantyClaim.Any())
            {
                return NotFound();
            }
            return Ok(WarrantyClaim);
        }

        [HttpGet("note/{note}")]
        public async Task<ActionResult<IEnumerable<WarrantyClaim>>> GetWarrantyClaimByNote(string note)
        {
            var WarrantyClaim = await _WarrantyClaimervice.GetWarrantyClaimByNoteAsync(note);
            if (WarrantyClaim == null || !WarrantyClaim.Any())
            {
                return NotFound();
            }
            return Ok(WarrantyClaim);
        }

        [HttpPost("create")]
        public async Task<ActionResult<WarrantyClaim>> CreateWarrantyClaim(
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes)
        {
            var warrantyClaim = await _WarrantyClaimervice.CreateWarrantyClaimAsync(partId, supplierId, claimDate, status, notes);
            if (warrantyClaim == null)
            {
                return BadRequest("Failed to create warranty claim.");
            }
            return CreatedAtAction(nameof(GetWarrantyClaimById), new { id = warrantyClaim.ClaimId }, warrantyClaim);
        }

        [HttpPut("update/{claimId}")]
        public async Task<ActionResult<WarrantyClaim>> UpdateWarrantyClaim(
            int claimId,
            int partId,
            int supplierId,
            DateOnly claimDate,
            string status,
            string notes)
        {
            var warrantyClaim = await _WarrantyClaimervice.UpdateWarrantyClaimAsync(claimId, partId, supplierId, claimDate, status, notes);
            if (warrantyClaim == null)
            {
                return NotFound();
            }
            return Ok(warrantyClaim);
        }

        [HttpDelete("deactivate/{claimId}")]
        public async Task<ActionResult<WarrantyClaim>> DeactivateWarrantyClaim(int claimId)
        {
            var warrantyClaim = await _WarrantyClaimervice.DeactivateWarrantyClaimAsync(claimId);
            if (warrantyClaim == null)
            {
                return NotFound();
            }
            return Ok(warrantyClaim);
        }
    }
}
