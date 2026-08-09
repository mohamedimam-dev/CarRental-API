using CarRental.API.Common;
using CarRental.API.Data;
using CarRental.API.DTOs.Users;
using CarRental.API.Entities;
using CarRental.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Services
{
    public class UserService : IUserService
    {
        private readonly CarRentalDbContext _context;

        public UserService(CarRentalDbContext context)
        {
            _context = context;
        }


        public async Task<ServiceResult<UserDTO>> AddAsync(AddUserDTO dto)
        {
            bool roleExists = await _context.Roles
                .AnyAsync(r => r.RoleId == dto.RoleId);

            if (!roleExists)
                return ServiceResult<UserDTO>
                    .NotFound("Role not found.");

            bool usernameExists = await _context.Users
                .AnyAsync(u => u.Username == dto.Username);

            if (usernameExists)
                return ServiceResult<UserDTO>
                    .Conflict("Username already exists.");

            User user = new User
            {
                FullName = dto.FullName,
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Email = dto.Email,
                Phone = dto.Phone,
                RoleId = dto.RoleId,
                IsActive = true
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            UserDTO userDto = new UserDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };

            return ServiceResult<UserDTO>.Success(userDto);
        }

        public async Task<ServiceResult<bool>> ChangePasswordAsync(
            int userId,
            ChangePasswordDTO dto)
        {
            User? user = await _context.Users
                .FindAsync(userId);

            if (user == null)
                return ServiceResult<bool>
                    .NotFound("User not found.");

            if (!user.IsActive)
                return ServiceResult<bool>
                    .Conflict("User account is inactive.");

            bool isPasswordCorrect =
                BCrypt.Net.BCrypt.Verify(
                    dto.CurrentPassword,
                    user.PasswordHash);

            if (!isPasswordCorrect)
                return ServiceResult<bool>
                    .BadRequest("Current password is incorrect.");

            user.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> DeactivateAsync(int userId)
        {
            User? user = await _context.Users
                .FindAsync(userId);

            if (user == null)
                return ServiceResult<bool>
                    .NotFound("User not found.");

            if (!user.IsActive)
                return ServiceResult<bool>
                    .Conflict("User is already inactive.");

            user.IsActive = false;

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Success(true);
        }
      
        public async Task<List<UserListDTO>> GetAllAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .OrderBy(u => u.FullName)
                .Select(u => new UserListDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Username = u.Username,
                    RoleId = u.RoleId,
                    IsActive = u.IsActive
                })
                .ToListAsync();
        }

        public async Task<ServiceResult<UserDTO>> GetByIdAsync(int userId)
        {
            UserDTO? user = await _context.Users
                .AsNoTracking()
                .Where(u => u.UserId == userId)
                .Select(u => new UserDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Username = u.Username,
                    Email = u.Email,
                    Phone = u.Phone,
                    RoleId = u.RoleId,
                    IsActive = u.IsActive,
                    CreatedDate = u.CreatedDate
                })
                .FirstOrDefaultAsync();

            if (user == null)
                return ServiceResult<UserDTO>
                    .NotFound("User not found.");

            return ServiceResult<UserDTO>.Success(user);
        }

        public async Task<ServiceResult<UserDTO>> UpdateAsync(
            int userId,
            UpdateUserDTO dto)
        {
            User? user = await _context.Users
                .FindAsync(userId);

            if (user == null)
                return ServiceResult<UserDTO>
                    .NotFound("User not found.");

            bool roleExists = await _context.Roles
                .AnyAsync(r => r.RoleId == dto.RoleId);

            if (!roleExists)
                return ServiceResult<UserDTO>
                    .NotFound("Role not found.");

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.RoleId = dto.RoleId;
            user.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();

            UserDTO userDto = new UserDTO
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };

            return ServiceResult<UserDTO>.Success(userDto);
        }

        public async Task<UserForLoginDTO?> GetUserForLoginAsync(string username)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Username == username)
                .Select(u => new UserForLoginDTO
                {
                    UserId = u.UserId,
                    FullName = u.FullName,
                    Username = u.Username,
                    PasswordHash = u.PasswordHash,
                    RoleId = u.RoleId,
                    Role = u.Role.RoleName,
                    IsActive = u.IsActive,

                    RefreshTokenHash = u.RefreshTokenHash,
                    RefreshTokenExpiresAt = u.RefreshTokenExpiresAt,
                    RefreshTokenRevokedAt = u.RefreshTokenRevokedAt
                })
                .FirstOrDefaultAsync();
        }
       
        public async Task<bool> UpdateRefreshTokenAsync(
           int userId,
           string refreshTokenHash,
           DateTime refreshTokenExpiresAt,
           DateTime? refreshTokenRevokedAt)
        {
            User? user = await _context.Users.FindAsync(userId);

            if (user == null)
                return false;

            user.RefreshTokenHash = refreshTokenHash;
            user.RefreshTokenExpiresAt = refreshTokenExpiresAt;
            user.RefreshTokenRevokedAt = refreshTokenRevokedAt;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RevokeRefreshTokenAsync(int userId)
        {
            User? user = await _context.Users.FindAsync(userId);

            if (user == null)
                return false;

            user.RefreshTokenRevokedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
