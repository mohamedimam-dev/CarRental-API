using CarRental.API.Common;
using CarRental.API.DTOs.RentalBookings;
using CarRental.API.DTOs.RentalBookings.CarRental.API.DTOs.RentalBookings;
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
    public class RentalBookingsController : ControllerBase
    {
        private readonly IRentalBookingService _rentalBookingService;

        public RentalBookingsController(IRentalBookingService rentalBookingService)
        {
            _rentalBookingService = rentalBookingService;
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            userId = 0;

            string? userIdClaim =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(userIdClaim, out userId);
        }

        [HttpGet("{bookingId}", Name = "GetRentalBookingById")]
        public async Task<ActionResult<RentalBookingDTO>> GetRentalBookingById(int bookingId)
        {
            if (bookingId <= 0)
                return BadRequest("Invalid Booking ID.");

            ServiceResult<RentalBookingDTO> result =
                await _rentalBookingService.GetByIdAsync(bookingId);

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
     
        [HttpPost("Add", Name = "AddRentalBooking")]
        public async Task<ActionResult<RentalBookingDTO>> AddRentalBooking(AddRentalBookingDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetCurrentUserId(out int createdByUserId))
                return Unauthorized();

            ServiceResult<RentalBookingDTO> result =
                await _rentalBookingService.AddAsync(
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
                        nameof(GetRentalBookingById),
                        new { bookingId = result.Data!.BookingId },
                        result.Data);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{bookingId}/Cancel", Name = "CancelRentalBooking")]
        public async Task<ActionResult> CancelRentalBooking(
          int bookingId)
        {
            if (bookingId <= 0)
                return BadRequest("Invalid Booking ID.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryGetCurrentUserId(out int cancelledByUserId))
                return Unauthorized();

            ServiceResult<bool> result =
                await _rentalBookingService.CancelAsync(
                    bookingId, 
                    cancelledByUserId);

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

        [HttpGet(Name = "GetAllRentalBookings")]
        public async Task<ActionResult<List<RentalBookingListDTO>>> GetAllRentalBookings()
        {
            List<RentalBookingListDTO> bookings =
                await _rentalBookingService.GetAllAsync();

            return Ok(bookings);
        }
    }
}
