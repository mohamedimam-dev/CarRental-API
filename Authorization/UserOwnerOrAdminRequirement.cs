using Microsoft.AspNetCore.Authorization;

namespace CarRental.API.Authorization
{
    public class UserOwnerOrAdminRequirement : IAuthorizationRequirement
    {
    }
}
