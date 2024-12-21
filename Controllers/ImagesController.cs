using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace Gigashop.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        [HttpGet("products")]
        public IActionResult GetProductImages()
        {
            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Products");

            if (!Directory.Exists(imageFolder))
            {
                return NotFound("Image folder not found.");
            }

            var images = Directory.GetFiles(imageFolder)
                                  .Select(Path.GetFileName)
                                  .ToList();

            return Ok(images);
        }
    }
}
