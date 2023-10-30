using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NFKApplication.Database;
using NFKApplication.Models;
using NFKApplication.Services;
using Serilog;
using System.Text.RegularExpressions;

namespace NFKApplication.Pages
{
    public class LogOverviewModel : PageModel
    {
        public List<Log> Logs { get; set; } = new List<Log>();

        private readonly ILogRepository _logRepository;

        public LogOverviewModel(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void OnGet()
        {
            Logs = _logRepository
                .GetLogs(50)
                .Select(logDto => new Log
                {
                    Id = logDto.Id,
                    Message = logDto.Message,
                    Level = logDto.Level,
                    Timestamp = logDto.Timestamp
                })
                .ToList();
        }
    }
    public class Log
    {
        public int Id { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
    }
}
