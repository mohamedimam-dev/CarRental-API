using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CarRental.API.DTOs.FuelType;
using Microsoft.AspNetCore.Authorization;

namespace CarRental.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FuelTypesController : ControllerBase
    {
        private readonly IFuelTypeService _fuelTypeService;

        public FuelTypesController(IFuelTypeService fuelTypeService)
        {
            _fuelTypeService = fuelTypeService;
        }

       
        [HttpGet(Name = "GetAllFuelTypes")]
        public async Task<ActionResult<List<FuelTypeDTO>>> GetAllFuelTypes()
        {
            List<FuelTypeDTO> fuelTypes =
                await _fuelTypeService.GetAllAsync();

            return Ok(fuelTypes);
        }
    }
}
