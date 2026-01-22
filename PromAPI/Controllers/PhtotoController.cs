using Microsoft.AspNetCore.Mvc;
using PhotoPromAPI.Models;

namespace PhotoPromAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoController : ControllerBase
    {
        //  tijdelijk (later database)
        private static List<Photo> Photos = new();
        private static int NextId = 1;

        //  Foto’s ophalen
        [HttpGet]
        public IActionResult GetPhotos()
        {
            return Ok(Photos.OrderByDescending(p => p.Tijd));
        }

        // Foto uploaden
        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Geen foto ontvangen.");

            // uploads map
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // unieke bestandsnaam
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            // bestand opslaan
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // metadata
            var photo = new Photo
            {
                Id = NextId++,
                FileName = fileName,
                Url = $"/uploads/{fileName}",
                Tijd = DateTime.Now,
                Gebruiker = "testUser" // later uit JWT halen
            };

            Photos.Add(photo);

            return Ok(photo);
        }
    }
}
