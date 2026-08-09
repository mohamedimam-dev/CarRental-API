using CarRental.API.Common;
using CarRental.API.DTOs.Users;

namespace CarRental.API.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserListDTO>> GetAllAsync();

        Task<ServiceResult<UserDTO>> GetByIdAsync(int userId);
     
        Task<ServiceResult<UserDTO>> AddAsync(AddUserDTO dto);

        Task<ServiceResult<UserDTO>> UpdateAsync(
            int userId,
            UpdateUserDTO dto);
      
        Task<ServiceResult<bool>> DeactivateAsync(int userId);
      
        Task<ServiceResult<bool>> ChangePasswordAsync(
            int userId,
            ChangePasswordDTO dto);

        Task<UserForLoginDTO?> GetUserForLoginAsync(string username);

        Task<bool> UpdateRefreshTokenAsync(
            int userId,
            string refreshTokenHash,
            DateTime refreshTokenExpiresAt,
            DateTime? refreshTokenRevokedAt);

        Task<bool> RevokeRefreshTokenAsync(int userId);
    }
}
