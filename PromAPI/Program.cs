using Microsoft.EntityFrameworkCore; // Required for Entity Framework
using PromAPI.Data; // Ensure this matches your AppDbContext namespace

var builder = WebApplication.CreateBuilder(args);

// --- SERVICES SECTION (Before builder.Build()) ---

// 1. Add support for controllers (required for API routes)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {   
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 2. Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 3. CORS Configuration (Allows Blazor to talk to API)
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 4. Authorization Service
builder.Services.AddAuthorization();

// ============================================================
// DATABASE CONNECTION 
// ============================================================

// Get connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register AppDbContext with MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ============================================================


var app = builder.Build();

// --- MIDDLEWARE SECTION (After builder.Build()) ---

// Enable Swagger in Development mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Найдем физический путь к папке wwwroot/approved
var approvedPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "approved");

// Создадим её, если сервер её не видит
if (!Directory.Exists(approvedPath))
{
    Directory.CreateDirectory(approvedPath);
}

var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
provider.Mappings[".jpg"] = "image/jpeg";
provider.Mappings[".jpeg"] = "image/jpeg";

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath, "approved")),
    RequestPath = "/approved",
    ContentTypeProvider = provider // Добавляем провайдер типов
});

app.UseStaticFiles();

// Enable CORS
app.UseCors("AllowAll");

// Enable Authorization
app.UseAuthorization();

// CRITICAL: Map controllers so the API endpoints work
app.MapControllers();

app.Run();