using CarRental.API.DTOs.Auth;
using CarRental.API.DTOs.Users;
using CarRental.API.Enums;
using CarRental.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly ISecurityLogService _securityLogService;

        public AuthController(
          IUserService userService,
          IConfiguration configuration,
          ISecurityLogService securityLogService)
        {
            _userService = userService;
            _configuration = configuration;
            _securityLogService = securityLogService;
        }

        private void LogSecurityEvent(
          enSecurityEventType eventType,
          int? userId)
        {
            string ipAddress =
                HttpContext.Connection.RemoteIpAddress?.ToString()
                ?? "Unknown";

            string endpoint = HttpContext.Request.Path;

            _securityLogService.AddLog(
                eventType.ToString(),
                userId,
                ipAddress,
                endpoint);
        }

        [HttpPost("Login")]
        [EnableRateLimiting("AuthLimiter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            UserForLoginDTO? user =
                await _userService.GetUserForLoginAsync(loginDto.Username);

            // التحقق من وجود المستخدم
            if (user == null)
            {
                LogSecurityEvent(
                    enSecurityEventType.LoginFailed,
                    null);

                return Unauthorized("Invalid username or password.");
            }

            // التحقق من كلمة المرور
            bool isValidPassword =
                BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

            if (!isValidPassword)
            {
                LogSecurityEvent(
                    enSecurityEventType.LoginFailed,
                    user.UserId);

                return Unauthorized("Invalid username or password.");
            }

            // التحقق من أن الحساب مفعل
            if (!user.IsActive)
            {
                LogSecurityEvent(
                    enSecurityEventType.InactiveAccount,
                    user.UserId);

                return Unauthorized("Your account is inactive.");
            }

            // إنشاء الـ Claims
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

            // قراءة إعدادات JWT من appsettings.json
            var secretKey = _configuration["JWT:SecretKey"];
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var expirationInMinutes =
                int.Parse(_configuration["JWT:ExpirationInMinutes"]!);

            // إنشاء مفتاح التوقيع
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey!));

            // بيانات التوقيع
            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            // إنشاء الـ JWT
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
                signingCredentials: credentials);

            // إنشاء Access Token
            string accessToken =
                new JwtSecurityTokenHandler().WriteToken(token);

            // إنشاء Refresh Token
            string refreshToken = GenerateRefreshToken();

            // عمل Hash للـ Refresh Token
            string refreshTokenHash =
                BCrypt.Net.BCrypt.HashPassword(refreshToken);

            // حفظ بيانات الـ Refresh Token
            bool updated =
                await _userService.UpdateRefreshTokenAsync(
                    user.UserId,
                    refreshTokenHash,
                    DateTime.UtcNow.AddDays(7),
                    null);

            if (!updated)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Failed to save refresh token.");
            }

            LogSecurityEvent(
                enSecurityEventType.LoginSucceeded,
                user.UserId);

            // إرجاع التوكنات
            return Ok(new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        [HttpPost("Refresh")]
        [EnableRateLimiting("AuthLimiter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh(
        [FromBody] RefreshRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            UserForLoginDTO? user =
                await _userService.GetUserForLoginAsync(request.UserName);

            if (user == null)
            {
                LogSecurityEvent(
                    enSecurityEventType.RefreshTokenFailed,
                    null);

                return Unauthorized("Invalid refresh request.");
            }

            if (user.RefreshTokenRevokedAt != null)
            {
                LogSecurityEvent(
                    enSecurityEventType.RefreshTokenRevoked,
                    user.UserId);

                return Unauthorized("Refresh token has been revoked.");
            }

            if (!user.RefreshTokenExpiresAt.HasValue ||
                user.RefreshTokenExpiresAt.Value <= DateTime.UtcNow)
            {
                LogSecurityEvent(
                    enSecurityEventType.RefreshTokenExpired,
                    user.UserId);

                return Unauthorized("Refresh token has expired.");
            }

            if (string.IsNullOrWhiteSpace(user.RefreshTokenHash))
            {
                LogSecurityEvent(
                    enSecurityEventType.RefreshTokenFailed,
                    user.UserId);

                return Unauthorized("Invalid refresh token.");
            }

            bool isValidRefreshToken =
                BCrypt.Net.BCrypt.Verify(
                    request.RefreshToken,
                    user.RefreshTokenHash);

            if (!isValidRefreshToken)
            {
                LogSecurityEvent(
                    enSecurityEventType.RefreshTokenFailed,
                    user.UserId);

                return Unauthorized("Invalid refresh token.");
            }

            // إنشاء Claims
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

            var secretKey = _configuration["JWT:SecretKey"];
            var issuer = _configuration["JWT:Issuer"];
            var audience = _configuration["JWT:Audience"];
            var expirationInMinutes =
                int.Parse(_configuration["JWT:ExpirationInMinutes"]!);

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secretKey!));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
                signingCredentials: credentials);

            string accessToken =
                new JwtSecurityTokenHandler().WriteToken(token);

            // ===== Refresh Token Rotation =====

            string newRefreshToken = GenerateRefreshToken();

            bool updated =
                await _userService.UpdateRefreshTokenAsync(
                    user.UserId,
                    BCrypt.Net.BCrypt.HashPassword(newRefreshToken),
                    DateTime.UtcNow.AddDays(7),
                    null);

            if (!updated)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Failed to update refresh token.");
            }

            LogSecurityEvent(
                enSecurityEventType.RefreshTokenSucceeded,
                user.UserId);

            return Ok(new TokenResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout(
        [FromBody] LogoutRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            UserForLoginDTO? user =
                await _userService.GetUserForLoginAsync(request.UserName);

            // لا نكشف هل المستخدم موجود أم لا
            if (user == null)
            {
                LogSecurityEvent(
                    enSecurityEventType.LogoutFailed,
                    null);

                return Ok();
            }

            if (string.IsNullOrWhiteSpace(user.RefreshTokenHash))
            {
                LogSecurityEvent(
                    enSecurityEventType.LogoutFailed,
                    user.UserId);

                return Ok();
            }

            bool isValidRefreshToken =
                BCrypt.Net.BCrypt.Verify(
                    request.RefreshToken,
                    user.RefreshTokenHash);

            if (!isValidRefreshToken)
            {
                LogSecurityEvent(
                    enSecurityEventType.LogoutFailed,
                    user.UserId);

                return Ok();
            }

            bool revoked =
                await _userService.RevokeRefreshTokenAsync(user.UserId);

            if (!revoked)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Failed to revoke refresh token.");
            }

            LogSecurityEvent(
                enSecurityEventType.LogoutSucceeded,
                user.UserId);

            return Ok("Logged out successfully.");
        }

    }
}
