using CarRental.API.Data;
using CarRental.API.Entities;
using CarRental.API.Services.Interfaces;

namespace CarRental.API.Services
{
    public class SecurityLogService : ISecurityLogService
    {
        private readonly CarRentalDbContext _context;

        public SecurityLogService(CarRentalDbContext context)
        {
            _context = context;
        }

        public bool AddLog(
             string eventType,
             int? userId,
             string ipAddress,
             string endpoint)
        {
            SecurityLog securityLog = new SecurityLog
            {
                EventType = eventType,
                UserId = userId,
                IPAddress = ipAddress,
                Endpoint = endpoint
            };

            _context.SecurityLogs.Add(securityLog);

            return _context.SaveChanges() > 0;
        }
    }
}
