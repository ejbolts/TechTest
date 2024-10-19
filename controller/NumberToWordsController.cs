using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace NumberToWordsApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class NumberToWordsController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public NumberToWordsController(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [HttpGet("/")]
        public IActionResult GetHtml()
        {
            var filePath = Path.Combine(_environment.WebRootPath, "index.html");
            if (System.IO.File.Exists(filePath))
            {
                return PhysicalFile(filePath, "text/html");
            }
            return NotFound("index.html not found.");
        }



    }
}
