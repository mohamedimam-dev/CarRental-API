using CarRental.API.Common;
using CarRental.API.DTOs.VehiclesReturn;
using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleReturnsController : ControllerBase
    {
        private readonly IVehicleReturnService _vehicleReturnService;

        public VehicleReturnsController(
            IVehicleReturnService vehicleReturnService)
        {
            _vehicleReturnService = vehicleReturnService;
        }

        [HttpGet("{returnId}", Name = "GetVehicleReturnById")]
        public async Task<ActionResult<VehicleReturnDTO>> GetVehicleReturnById(int returnId)
        {
            if (returnId <= 0)
                return BadRequest("Invalid Return ID.");

            ServiceResult<VehicleReturnDTO> result =
                await _vehicleReturnService.GetByIdAsync(returnId);

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


        [HttpGet("Booking/{bookingId}",
        Name = "GetVehicleReturnByBookingId")]
        public async Task<ActionResult<VehicleReturnDTO>>
        GetVehicleReturnByBookingId(int bookingId)
        {
            if (bookingId <= 0)
                return BadRequest("Invalid Booking ID.");

            ServiceResult<VehicleReturnDTO> result =
                await _vehicleReturnService.GetByBookingIdAsync(bookingId);

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

       
        [HttpPost("Add", Name = "AddVehicleReturn")]
        public async Task<ActionResult<VehicleReturnDTO>> AddVehicleReturn(AddVehicleReturnDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ServiceResult<VehicleReturnDTO> result =
                await _vehicleReturnService.AddAsync(dto);

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
                        nameof(GetVehicleReturnById),
                        new { returnId = result.Data!.ReturnId },
                        result.Data);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

      
        [HttpGet(Name = "GetAllVehicleReturns")]
        public async Task<ActionResult<List<VehicleReturnListDTO>>>
            GetAllVehicleReturns()
        {
            List<VehicleReturnListDTO> vehicleReturns =
                await _vehicleReturnService.GetAllAsync();

            return Ok(vehicleReturns);
        }
    }
}
