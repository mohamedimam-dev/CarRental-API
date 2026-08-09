
using CarRental.API.Common;
using CarRental.API.DTOs.Customers;
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
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IAuditLogService _auditLogService;
        public CustomersController(
          ICustomerService customerService,
          IAuditLogService auditLogService)
        {
            _customerService = customerService;
            _auditLogService = auditLogService;
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            userId = 0;

            string? userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(userIdClaim, out userId);
        }

        [HttpGet("All", Name = "GetAllCustomers")]
        public async Task<ActionResult<List<CustomerListDTO>>> GetAllCustomers()
        {
            List<CustomerListDTO> customers =
                await _customerService.GetAllAsync();

            return Ok(customers);
        }

      
        [HttpGet("{customerId}", Name = "GetCustomerById")]
        public async Task<ActionResult<CustomerDTO>> GetCustomerById(int customerId)
        {
            if (customerId <= 0)
                return BadRequest("Invalid Customer ID.");

            ServiceResult<CustomerDTO> result =
                await _customerService.GetByIdAsync(customerId);

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

        [HttpPost("Add", Name = "AddCustomer")]
        public async Task<ActionResult<CustomerDTO>> AddCustomer(AddCustomerDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetCurrentUserId(out int createdByUserId))
                return Unauthorized();

            ServiceResult<CustomerDTO> result =
                await _customerService.AddAsync(dto, createdByUserId);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    return CreatedAtAction(
                        nameof(GetCustomerById),
                        new { customerId = result.Data!.CustomerId },
                        result.Data);

                default:
                    return StatusCode(
                        StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Administrator, Manager")]
        [HttpPut("{customerId}", Name = "UpdateCustomer")]
        public async Task<ActionResult<CustomerDTO>> UpdateCustomer(
        int customerId,
        UpdateCustomerDTO dto)
        {
            if (customerId <= 0)
                return BadRequest("Invalid Customer ID.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ServiceResult<CustomerDTO> result =
                await _customerService.UpdateAsync(customerId, dto);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:

                    if (!TryGetCurrentUserId(out int userId))
                        return Unauthorized();

                    _auditLogService.Add(
                        userId,
                        enAuditAction.Update.ToString(),
                        enAuditEntity.Customer.ToString(),
                        customerId,
                        HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown");

                    return Ok(result.Data);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{customerId}", Name = "DeleteCustomer")]
        public async Task<ActionResult> DeleteCustomer(int customerId)
        {
            if (customerId <= 0)
                return BadRequest("Invalid Customer ID.");

            ServiceResult<bool> result =
                await _customerService.DeleteAsync(customerId);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    if (!TryGetCurrentUserId(out int userId))
                        return Unauthorized();

                    _auditLogService.Add(
                        userId,
                        enAuditAction.Delete.ToString(),
                        enAuditEntity.Customer.ToString(),
                        customerId,
                        HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown");
                    return NoContent();


                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
   
    }
}
