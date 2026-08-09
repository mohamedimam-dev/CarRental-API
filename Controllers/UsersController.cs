using CarRental.API.Common;
using CarRental.API.DTOs.Users;
using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet(Name = "GetAllUsers")]
        public async Task<ActionResult<List<UserListDTO>>> GetAllUsers()
        {
            List<UserListDTO> users =
                await _userService.GetAllAsync();

            return Ok(users);
        }


        [HttpGet("{userId}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetUserById(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid User ID.");

            ServiceResult<UserDTO> result =
                await _userService.GetByIdAsync(userId);

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
        [HttpPost(Name = "AddUser")]
        public async Task<ActionResult<UserDTO>> AddUser(AddUserDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ServiceResult<UserDTO> result =
                await _userService.AddAsync(dto);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    return CreatedAtRoute(
                        "GetUserById",
                        new { userId = result.Data!.UserId },
                        result.Data);

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{userId}", Name = "UpdateUser")]
        public async Task<ActionResult<UserDTO>> UpdateUser(
        int userId,
        UpdateUserDTO dto)
        {
            if (userId <= 0)
                return BadRequest("Invalid User ID.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ServiceResult<UserDTO> result =
                await _userService.UpdateAsync(userId, dto);

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
        [HttpPut("{userId}/Deactivate", Name = "DeactivateUser")]
        public async Task<ActionResult> DeactivateUser(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid User ID.");

            ServiceResult<bool> result =
                await _userService.DeactivateAsync(userId);

            switch (result.Status)
            {
                case ServiceResultStatus.NotFound:
                    return NotFound(result.Message);

                case ServiceResultStatus.Conflict:
                    return Conflict(result.Message);

                case ServiceResultStatus.Success:
                    return NoContent();

                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPut("{userId}/ChangePassword", Name = "ChangePassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> ChangePassword(
         int userId,
         ChangePasswordDTO dto)
        {
            if (userId <= 0)
                return BadRequest("Invalid User ID.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ServiceResult<bool> result =
                await _userService.ChangePasswordAsync(userId, dto);

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
