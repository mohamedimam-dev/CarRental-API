using CarRental.API.Common;
using CarRental.API.DTOs.Vehicles;
using CarRental.API.Entities;
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
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IAuditLogService _auditLogService;

        public VehiclesController(
          IVehicleService vehicleService,
          IAuditLogService auditLogService)
        {
            _vehicleService = vehicleService;
            _auditLogService = auditLogService;
        }
        private bool TryGetCurrentUserId(out int userId)
        {
            userId = 0;

            string? userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(userIdClaim, out userId);
        }

        [HttpGet(Name = "GetAllVehicles")]
        public async Task<ActionResult<List<VehicleListDTO>>> GetAllVehicles()
        {
            var vehicles = await _vehicleService.GetAllAsync();

            return Ok(vehicles);
        }

        [HttpGet("{vehicleId}", Name = "GetVehicleById")]
        public async Task<ActionResult<VehicleDTO>> GetVehicleById(int vehicleId)
        {
            if (vehicleId <= 0)
                return BadRequest("Vehicle ID must be greater than zero.");

            ServiceResult<VehicleDTO> result =
                await _vehicleService.GetByIdAsync(vehicleId);

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

        [Authorize(Roles = "Administrator")]
        [HttpPost("Add", Name = "AddVehicle")]
        public async Task<ActionResult<VehicleDTO>> AddVehicle(AddVehicleDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetCurrentUserId(out int createdByUserId))
                return Unauthorized();

            ServiceResult<VehicleDTO> result =
                await _vehicleService.AddAsync(dto, createdByUserId);

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
                        nameof(GetVehicleById),
                        new { vehicleId = result.Data!.VehicleId },
                        result.Data);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{vehicleId}", Name = "UpdateVehicle")]
        public async Task<ActionResult<VehicleDTO>> UpdateVehicle(int vehicleId, UpdateVehicleDTO dto)
        {
            if (vehicleId <= 0)
                return BadRequest("Vehicle ID must be greater than zero.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ServiceResult<VehicleDTO> result =
                await _vehicleService.UpdateAsync(vehicleId, dto);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.BadRequest:
                    return BadRequest(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    string? userIdClaim =
                       User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (int.TryParse(userIdClaim, out int currentUserId))
                    {
                        _auditLogService.Add(
                            currentUserId,
                            enAuditAction.Update.ToString(),
                            enAuditEntity.Vehicle.ToString(),
                            vehicleId,
                            HttpContext.Connection.RemoteIpAddress?.ToString()
                                ?? "Unknown");
                    }

                    return Ok(result.Data);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{vehicleId}", Name = "DeleteVehicle")]
        public async Task<ActionResult> DeleteVehicle(int vehicleId)
        {
            if (vehicleId <= 0)
                return BadRequest("Vehicle ID must be greater than zero.");

            ServiceResult<bool> result =
                await _vehicleService.DeleteAsync(vehicleId);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    string? userIdClaim =
                       User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (int.TryParse(userIdClaim, out int currentUserId))
                    {
                        _auditLogService.Add(
                            currentUserId,
                            enAuditAction.Delete.ToString(),
                            enAuditEntity.Vehicle.ToString(),
                            vehicleId,
                            HttpContext.Connection.RemoteIpAddress?.ToString()
                                ?? "Unknown");
                    }

                    return NoContent();

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
