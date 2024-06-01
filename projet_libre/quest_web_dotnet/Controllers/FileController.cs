using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Text;
using System.Globalization;
using System.Linq;

namespace quest_web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FileController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath ?? string.Empty, "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a random file name without accents or special characters
            var randomFileName = RemoveAccentsAndSpecialCharacters(Guid.NewGuid().ToString());
            var extension = Path.GetExtension(file.FileName);
            var newFileName = $"{randomFileName}{extension}";

            var filePath = Path.Combine(uploadsFolder, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/images/{newFileName}";
            return Ok(new { Url = fileUrl });
        }

        private string RemoveAccentsAndSpecialCharacters(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            var cleanString = new string(stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC)
                .Where(c => char.IsLetterOrDigit(c))
                .ToArray());

            return cleanString;
        }
    }
}
