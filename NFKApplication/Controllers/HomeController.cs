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
            var filePath = string.Empty;
            var fileBytes = new byte[0];
            try
            {
                filePath = new Uri(imageUrl).LocalPath;
                fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "image/jpeg");
            }
            catch (Exception ex)
            {
                return BadRequest($"Fetching image at path {filePath} failed with reason {ex.Message}. Content: {Convert.ToBase64String(fileBytes)}");
            }
        }
    }
}
