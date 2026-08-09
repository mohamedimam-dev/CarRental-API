using CarRental.API.Common;
using CarRental.API.DTOs.RentalTransactions;
using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RentalTransactionsController : ControllerBase
    {
        private readonly IRentalTransactionService _rentalTransactionService;

        public RentalTransactionsController(IRentalTransactionService rentalTransactionService)
        {
            _rentalTransactionService = rentalTransactionService;
        }

        [HttpGet("{transactionId}", Name = "GetRentalTransactionById")]
        public async Task<ActionResult<RentalTransactionDTO>> GetRentalTransactionById(int transactionId)
        {
            if (transactionId <= 0)
                return BadRequest("Invalid Transaction ID.");

            ServiceResult<RentalTransactionDTO> result =
                await _rentalTransactionService.GetByIdAsync(transactionId);

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

        [HttpGet("ByBooking/{bookingId}", Name = "GetRentalTransactionByBookingId")]
        public async Task<ActionResult<RentalTransactionDTO>> GetRentalTransactionByBookingId(int bookingId)
        {
            if (bookingId <= 0)
                return BadRequest("Invalid Booking ID.");

            ServiceResult<RentalTransactionDTO> result =
                await _rentalTransactionService.GetByBookingIdAsync(bookingId);

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
     
        [HttpGet(Name = "GetAllRentalTransactions")]
        public async Task<ActionResult<List<RentalTransactionListDTO>>> GetAllRentalTransactions()
        {
            List<RentalTransactionListDTO> transactions =
                await _rentalTransactionService.GetAllAsync();

            return Ok(transactions);
        }
    }
}
