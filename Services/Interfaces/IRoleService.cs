using CarRental.API.DTOs.Roles;

namespace CarRental.API.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<RoleDTO>> GetAllAsync();

    }
}
