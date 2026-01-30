var builder = WebApplication.CreateBuilder(args);

// 1. Настраиваем политику
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
var app = builder.Build();

// 2. Включаем её СТРОГО перед MapControllers
app.UseCors("AllowAll");

app.UseStaticFiles();
app.MapControllers();
app.Run();