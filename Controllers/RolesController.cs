using CarRental.API.DTOs.Roles;
using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {

        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet(Name = "GetAllRoles")]
        public async Task<ActionResult<List<RoleDTO>>> GetAllRoles()
        {
            List<RoleDTO> roles =
                await _roleService.GetAllAsync();

            return Ok(roles);
        }
    }
}
