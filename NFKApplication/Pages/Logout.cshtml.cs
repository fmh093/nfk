using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NFKApplication.Services;

namespace NFKApplication.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;
        private readonly IAuthService _authService;

        public LogoutModel(ILogger<LogoutModel> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        public void OnGet()
        {
            _authService.Logout(HttpContext);
        }
    }
}