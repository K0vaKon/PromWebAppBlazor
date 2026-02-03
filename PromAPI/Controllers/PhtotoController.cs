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

    [HttpGet]
    public IActionResult GetPhotos()
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        if (!Directory.Exists(folderPath)) return Ok(new List<string>());

        // Получаем список имен файлов из папки
        var files = Directory.GetFiles(folderPath)
                             .Select(Path.GetFileName)
                             .ToList();

        return Ok(files);
    }

    [HttpGet("pending")]
    public IActionResult GetPendingPhotos()
    {
        // Проверь, что _env не равен null
        if (_env.WebRootPath == null)
        {
            return BadRequest("WebRootPath is not configured");
        }

        var path = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(path))
        {
            return Ok(new List<string>());
        }

        var files = Directory.GetFiles(path)
                             .Select(Path.GetFileName)
                             .ToList();

        return Ok(files);
    }

    // Метод для одобрения: переносит файл из uploads в approved
    [HttpPost("approve/{fileName}")]
    public IActionResult ApprovePhoto(string fileName)
    {
        var sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
        var destPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "approved", fileName);

        if (System.IO.File.Exists(sourcePath))
        {
            // Создаем папку approved, если её нет
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "approved"));

            System.IO.File.Move(sourcePath, destPath); // Перемещаем файл
            return Ok();
        }
        return NotFound();
    }
}