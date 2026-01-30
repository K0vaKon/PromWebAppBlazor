using Microsoft.AspNetCore.Mvc;
using PhotoPromAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class PhotoController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public PhotoController(IWebHostEnvironment env)
    {
        _env = env; // Это позволяет получить путь к wwwroot автоматически
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null) return BadRequest("Файла нет");

        // Используем путь именно к wwwroot
        var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var path = Path.Combine(uploadsPath, file.FileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return Ok();
    }
}