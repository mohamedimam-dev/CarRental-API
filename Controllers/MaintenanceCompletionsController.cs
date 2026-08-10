using CarRental.API.Common;
using CarRental.API.DTOs.MaintenanceCompletion;
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
    public class MaintenanceCompletionsController : ControllerBase
    {
        private readonly IMaintenanceCompletionService _maintenanceCompletionService;

        public MaintenanceCompletionsController(
            IMaintenanceCompletionService maintenanceCompletionService)
        {
            _maintenanceCompletionService = maintenanceCompletionService;
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            userId = 0;

            string? userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(userIdClaim, out userId);
        }


        [HttpGet("{completionId}", Name = "GetMaintenanceCompletionById")]
        public async Task<ActionResult<MaintenanceCompletionDTO>> GetMaintenanceCompletionById(
        int completionId)
        {
            if (completionId <= 0)
                return BadRequest("Invalid Completion ID.");

            ServiceResult<MaintenanceCompletionDTO> result =
                await _maintenanceCompletionService.GetByIdAsync(completionId);

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

       
        [HttpGet(Name = "GetAllMaintenanceCompletions")]
        public async Task<ActionResult<List<MaintenanceCompletionListDTO>>> GetAllMaintenanceCompletions()
        {
            List<MaintenanceCompletionListDTO> completions =
                await _maintenanceCompletionService.GetAllAsync();

            return Ok(completions);
        }

        [Authorize(Roles = "Administrator, Manager")]
        [HttpPost("Add", Name = "AddMaintenanceCompletion")]
        public async Task<ActionResult<MaintenanceCompletionDTO>> AddMaintenanceCompletion(
          AddMaintenanceCompletionDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetCurrentUserId(out int createdByUserId))
                return Unauthorized();

            ServiceResult<MaintenanceCompletionDTO> result =
                await _maintenanceCompletionService.AddAsync(
                    dto,
                    createdByUserId);

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
                        nameof(GetMaintenanceCompletionById),
                        new { completionId = result.Data!.CompletionId },
                        result.Data);

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Administrator, Manager")]
        [HttpPut("{completionId}", Name = "UpdateMaintenanceCompletion")]
        public async Task<ActionResult<MaintenanceCompletionDTO>> UpdateMaintenanceCompletion(
            int completionId,
            UpdateMaintenanceCompletionDTO dto)
        {
            if (completionId <= 0)
                return BadRequest("Invalid Completion ID.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetCurrentUserId(out int updatedByUserId))
                return Unauthorized();

            ServiceResult<MaintenanceCompletionDTO> result =
                await _maintenanceCompletionService.UpdateAsync(
                    completionId,
                    dto,
                    updatedByUserId);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.BadRequest:
                    return BadRequest(result.Message);

                case ServiceResultStatus.Success:
                    return Ok(result.Data);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    
    }
}
