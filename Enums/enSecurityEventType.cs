namespace CarRental.API.Enums
{
    public enum enSecurityEventType
    {
        LoginSucceeded,
        LoginFailed,
        Forbidden,
        InactiveAccount,
        RateLimitExceeded,
        RefreshTokenFailed,
        RefreshTokenRevoked,
        RefreshTokenExpired,
        RefreshTokenSucceeded,
        LogoutSucceeded,
        LogoutFailed,
        InvalidToken
    }
}
