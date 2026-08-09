namespace CarRental.API.DTOs.Users
{
    public class UserListDTO
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public bool IsActive { get; set; }
    }
}
