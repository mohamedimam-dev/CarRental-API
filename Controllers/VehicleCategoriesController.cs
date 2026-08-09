using CarRental.API.DTOs.VehicleCategory;
using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleCategoriesController : ControllerBase
    {

        private readonly IVehicleCategoryService _vehicleCategoryService;

        public VehicleCategoriesController(
            IVehicleCategoryService vehicleCategoryService)
        {
            _vehicleCategoryService = vehicleCategoryService;
        }


        [HttpGet(Name = "GetAllVehicleCategories")]
        public async Task<ActionResult<List<VehicleCategoryDTO>>> GetAllVehicleCategories()
        {
            List<VehicleCategoryDTO> categories =
                await _vehicleCategoryService.GetAllAsync();

            return Ok(categories);
        }
    }
}
