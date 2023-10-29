using System.Text.RegularExpressions;

namespace NFKApplication.Database.Models
{
    public class LogEntryDto
    {
        public int Id { get; set; }
        public string Timestamp { get; set; }
        public string Level { get; set; }
        public string Exception { get; set; }
        public string RenderedMessage { get; set; }
        public string Properties { get; set; }
        public string Message => Regex.Unescape(RenderedMessage);
    }
}
