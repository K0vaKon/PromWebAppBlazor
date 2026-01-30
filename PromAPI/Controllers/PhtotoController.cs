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
        // Путь будет: ТвойПроект/wwwroot/uploads
        var uploads = Path.Combine(_env.WebRootPath, "uploads");

        if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);

        var path = Path.Combine(uploads, file.FileName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok();
    }
}