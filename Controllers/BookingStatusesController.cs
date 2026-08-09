using CarRental.API.DTOs.BookingStatus;
using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CarRental.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingStatusesController : ControllerBase
    {

        private readonly IBookingStatusService _bookingStatusService;

        public BookingStatusesController(
            IBookingStatusService bookingStatusService)
        {
            _bookingStatusService = bookingStatusService;
        }

       
        [HttpGet(Name = "GetAllBookingStatus")]
        public async Task<ActionResult<List<BookingStatusDTO>>> GetAllBookingStatus()
        {
            List<BookingStatusDTO> bookingStatus =
                await _bookingStatusService.GetAllAsync();

            return Ok(bookingStatus);
        }
    }
}
