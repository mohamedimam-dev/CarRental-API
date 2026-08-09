namespace CarRental.API.Services.Interfaces
{
    public interface IAuditLogService
    {
        bool Add(
          int userId,
          string action,
          string entityName,
          int entityId,
          string ipAddress);
    }
}
