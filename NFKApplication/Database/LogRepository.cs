using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NFKApplication.Database.Models;
using NFKApplication.Models;
using System.Diagnostics.CodeAnalysis;

namespace NFKApplication.Database
{
    public class LogRepository : ILogRepository
    {

        private readonly AppDbContext _context;

        public LogRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<LogEntryDto> GetLogs(int take)
        {
            return _context.Logs
                           .Where(log => log.Level != "Information")
                           .OrderByDescending(log => log.Timestamp)
                           .Take(take)
                           .ToList();
        }
    }

    public interface ILogRepository
    {
        List<LogEntryDto> GetLogs(int take);
    }
}
