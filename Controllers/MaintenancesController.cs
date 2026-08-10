using CarRental.API.Common;
using CarRental.API.DTOs.Maintenance;
using CarRental.API.Enums;
using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRental.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenancesController : ControllerBase
    {
        private readonly IMaintenanceService _maintenanceService;
        private readonly IAuditLogService _auditLogService;

        public MaintenancesController(
          IMaintenanceService maintenanceService,
          IAuditLogService auditLogService)
        {
            _maintenanceService = maintenanceService;
            _auditLogService = auditLogService;
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            userId = 0;

            string? userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(userIdClaim, out userId);
        }


        [HttpGet("{maintenanceId}", Name = "GetMaintenanceById")]
        public async Task<ActionResult<MaintenanceDTO>> GetMaintenanceById(int maintenanceId)
        {
            if (maintenanceId <= 0)
                return BadRequest("Invalid Maintenance ID.");

            ServiceResult<MaintenanceDTO> result =
                await _maintenanceService.GetByIdAsync(maintenanceId);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Success:
                    return Ok(result.Data);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        
        [HttpGet(Name = "GetAllMaintenance")]
        public async Task<ActionResult<List<MaintenanceListDTO>>> GetAllMaintenance()
        {
            List<MaintenanceListDTO> maintenances =
                await _maintenanceService.GetAllAsync();

            return Ok(maintenances);
        }


        [HttpPost("Add", Name = "AddMaintenance")]
        public async Task<ActionResult<MaintenanceDTO>> AddMaintenance(
          AddMaintenanceDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetCurrentUserId(out int createdByUserId))
                return Unauthorized();

            ServiceResult<MaintenanceDTO> result =
                await _maintenanceService.AddAsync(dto, createdByUserId);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.BadRequest:
                    return BadRequest(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    return CreatedAtAction(
                        nameof(GetMaintenanceById),
                        new { maintenanceId = result.Data!.MaintenanceId },
                        result.Data);

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Administrator, Manager")]
        [HttpPut("{maintenanceId}", Name = "UpdateMaintenance")]
        public async Task<ActionResult<MaintenanceDTO>> UpdateMaintenance(
        int maintenanceId,
        UpdateMaintenanceDTO dto)
        {
            if (maintenanceId <= 0)
                return BadRequest("Invalid Maintenance ID.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetCurrentUserId(out int updatedByUserId))
                return Unauthorized();

            ServiceResult<MaintenanceDTO> result =
                await _maintenanceService.UpdateAsync(
                    maintenanceId, dto, updatedByUserId);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Success:
                    _auditLogService.Add(
                       updatedByUserId,
                       enAuditAction.Update.ToString(),
                       enAuditEntity.Maintenance.ToString(),
                       maintenanceId,
                       HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
                    return Ok(result.Data);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Administrator, Manager")]
        [HttpPut("{maintenanceId}/Cancel", Name = "CancelMaintenance")]
        public async Task<ActionResult> CancelMaintenance(
        int maintenanceId)
        {
            if (maintenanceId <= 0)
                return BadRequest("Invalid Maintenance ID.");
           
            if (!TryGetCurrentUserId(out int cancelledByUserId))
                return Unauthorized();

            ServiceResult<bool> result =
                await _maintenanceService.CancelAsync(
                    maintenanceId, cancelledByUserId);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.BadRequest:
                    return BadRequest(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    return NoContent();

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
    }
}
