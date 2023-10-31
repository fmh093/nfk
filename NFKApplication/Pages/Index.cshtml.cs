using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NFKApplication.Database;

namespace NFKApplication.Pages
{
    public class IndexModel : PageModel
    {
        public string LogoPath = string.Empty;
        public IndexModel()
        {
        }

        public void OnGet()
        {
            LogoPath = PathHelper.ImagesPath;
        }
    }
}