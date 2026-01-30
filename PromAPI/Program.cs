var builder = WebApplication.CreateBuilder(args);

// 1. Добавляем политику CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin() // Разрешаем запросы с любого адреса
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 2. ВКЛЮЧАЕМ (это должно быть выше чем MapControllers)
app.UseCors();

app.UseStaticFiles();
app.MapControllers();
app.Run();