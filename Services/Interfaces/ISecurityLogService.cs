namespace CarRental.API.Services.Interfaces
{
    public interface ISecurityLogService
    {
        bool AddLog(
           string eventType,
           int? userId,
           string ipAddress,
           string endpoint);
    }
}
