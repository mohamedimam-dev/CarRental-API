using CarRental.API.Data;
using CarRental.API.Entities;
using CarRental.API.Services.Interfaces;

namespace CarRental.API.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly CarRentalDbContext _context;

        public AuditLogService(CarRentalDbContext context)
        {
            _context = context;
        }

        public bool Add(
            int userId,
            string action,
            string entityName,
            int entityId,
            string ipAddress)
        {
            AuditLog auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                IPAddress = ipAddress
            };

            _context.AuditLogs.Add(auditLog);

            return _context.SaveChanges() > 0;
        }
    }
}
