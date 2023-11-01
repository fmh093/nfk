using Microsoft.AspNetCore.Mvc;
using NFKApplication.Services;

namespace NFKApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }        

        [HttpGet("GetImage")]
        public IActionResult FetchImage(string imageUrl)
        {
            try
            {
                var filePath = new Uri(imageUrl).LocalPath;
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                return BadRequest($"Fetching image at path {imageUrl} failed with reason {ex.Message}. StackTrace: {ex.StackTrace}");
            }
        }
    }
}
