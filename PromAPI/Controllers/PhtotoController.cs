using Microsoft.AspNetCore.Mvc;
using PhotoPromAPI.Models; // Ensure you have this namespace or remove if not used yet

[ApiController]
[Route("api/[controller]")]
public class PhotoController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public PhotoController(IWebHostEnvironment env)
    {
        _env = env; // This allows getting the path to wwwroot automatically
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null) return BadRequest("File is missing");

        // Use the path specifically to wwwroot/uploads
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
        // Now looking into the approved folder!
        var path = Path.Combine(_env.WebRootPath, "approved");

        // If the folder doesn't exist yet (no photos approved), return an empty list
        if (!Directory.Exists(path))
            return Ok(new List<string>());

        var files = Directory.GetFiles(path)
                             .Select(Path.GetFileName)
                             .ToList();

        return Ok(files);
    }

    [HttpGet("pending")]
    public IActionResult GetPendingPhotos()
    {
        // Check that _env is not null
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

    // Method for approval: moves file from uploads to approved
    [HttpPost("approve/{fileName}")]
    public IActionResult ApprovePhoto(string fileName)
    {
        // It is better to use _env.WebRootPath instead of Directory.GetCurrentDirectory() for consistency
        var sourcePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
        var destPath = Path.Combine(_env.WebRootPath, "approved", fileName);

        if (System.IO.File.Exists(sourcePath))
        {
            // Create approved folder if it doesn't exist
            var approvedDir = Path.Combine(_env.WebRootPath, "approved");
            if (!Directory.Exists(approvedDir)) Directory.CreateDirectory(approvedDir);

            System.IO.File.Move(sourcePath, destPath); // Move the file
            return Ok();
        }
        return NotFound();
    }

    // Method for rejection: moves file from uploads to rejected
    [HttpPost("reject/{fileName}")]
    public IActionResult RejectPhoto(string fileName)
    {
        var sourcePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
        var destPath = Path.Combine(_env.WebRootPath, "rejected", fileName);

        if (System.IO.File.Exists(sourcePath))
        {
            // Create rejected folder if it doesn't exist
            var rejectedDir = Path.Combine(_env.WebRootPath, "rejected");
            if (!Directory.Exists(rejectedDir)) Directory.CreateDirectory(rejectedDir);

            System.IO.File.Move(sourcePath, destPath); // Move the file
            return Ok();
        }
        return NotFound();
    }
}