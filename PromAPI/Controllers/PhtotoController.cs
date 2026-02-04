using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoPromAPI.Models;
using PromAPI.Data;   // Проверь namespace
using PromAPI.Models; // Проверь namespace

[ApiController]
[Route("api/[controller]")]
public class PhotoController : ControllerBase
{
    private readonly IWebHostEnvironment _env;
    private readonly AppDbContext _context;

    public PhotoController(IWebHostEnvironment env, AppDbContext context)
    {
        _env = env;
        _context = context;
    }

    // 1. ЗАГРУЗКА
    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null) return BadRequest("File is missing");

        // Физическое сохранение
        var uploadsPath = Path.Combine(_env.WebRootPath, "uploads");
        if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);

        var path = Path.Combine(uploadsPath, file.FileName);
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Сохранение в БД
        // Ищем отправителя или создаем Гостя
        var defaultSender = await _context.Zenders.FirstOrDefaultAsync();
        if (defaultSender == null)
        {
            defaultSender = new Zender { Name = "Guest", Email = "guest@prombal.com" };
            _context.Zenders.Add(defaultSender);
            await _context.SaveChangesAsync();
        }

        var newPhoto = new Photo
        {
            FileName = file.FileName,
            Tijd = DateTime.Now,
            IsApproved = 0, // Статус 0 = Ожидает
            ZenderId = defaultSender.Id
        };

        _context.Photos.Add(newPhoto);
        await _context.SaveChangesAsync();

        return Ok();
    }

    // 2. ГАЛЕРЕЯ (Только одобренные)
    [HttpGet]
    public async Task<IActionResult> GetPhotos()
    {
        var photos = await _context.Photos
            .Include(p => p.Sender)
            .Where(p => p.IsApproved == 1) // Статус 1 = Одобрено
            .ToListAsync();

        return Ok(photos);
    }

    // 3. АДМИНКА (Только ожидающие)
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingPhotos()
    {
        var pending = await _context.Photos
            .Include(p => p.Sender)
            .Where(p => p.IsApproved == 0) // Статус 0 = Ожидает
            .ToListAsync();

        return Ok(pending);
    }

    // 4. ОДОБРЕНИЕ
    [HttpPost("approve/{fileName}")]
    public async Task<IActionResult> ApprovePhoto(string fileName)
    {
        // Перемещение файла
        var sourcePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
        var destPath = Path.Combine(_env.WebRootPath, "approved", fileName);

        if (System.IO.File.Exists(sourcePath))
        {
            var approvedDir = Path.Combine(_env.WebRootPath, "approved");
            if (!Directory.Exists(approvedDir)) Directory.CreateDirectory(approvedDir);
            System.IO.File.Move(sourcePath, destPath);
        }

        // Обновление БД
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.FileName == fileName);
        if (photo != null)
        {
            photo.IsApproved = 1; // Ставим статус 1
            await _context.SaveChangesAsync();
        }

        return Ok();
    }

    // 5. ОТКЛОНЕНИЕ (ИЗМЕНЕНО: теперь не удаляем запись)
    [HttpPost("reject/{fileName}")]
    public async Task<IActionResult> RejectPhoto(string fileName)
    {
        // Перемещение файла в папку rejected
        var sourcePath = Path.Combine(_env.WebRootPath, "uploads", fileName);
        var destPath = Path.Combine(_env.WebRootPath, "rejected", fileName);

        if (System.IO.File.Exists(sourcePath))
        {
            var rejectedDir = Path.Combine(_env.WebRootPath, "rejected");
            if (!Directory.Exists(rejectedDir)) Directory.CreateDirectory(rejectedDir);
            System.IO.File.Move(sourcePath, destPath);
        }

        // Обновление БД
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.FileName == fileName);
        if (photo != null)
        {
            // МЫ НЕ УДАЛЯЕМ ЗАПИСЬ!
            // Мы просто ставим статус "Отклонено" (например, цифра 2)
            photo.IsApproved = 2;
            await _context.SaveChangesAsync();
        }

        return Ok();
    }
}