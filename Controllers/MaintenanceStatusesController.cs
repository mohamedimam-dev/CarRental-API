using CarRental.API.DTOs.MaintenanceStatus;
using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceStatusesController : ControllerBase
    {

        private readonly IMaintenanceStatusService _maintenanceStatusService;

        public MaintenanceStatusesController(
            IMaintenanceStatusService maintenanceStatusService)
        {
            _maintenanceStatusService = maintenanceStatusService;
        }

        [HttpGet(Name = "GetAllMaintenanceStatuses")]
        public async Task<ActionResult<List<MaintenanceStatusDTO>>> GetAllMaintenanceStatuses()
        {
            List<MaintenanceStatusDTO> statuses =
                await _maintenanceStatusService.GetAllAsync();

            return Ok(statuses);
        }
    }
}
