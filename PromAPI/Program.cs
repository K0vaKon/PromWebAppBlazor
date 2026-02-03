var builder = WebApplication.CreateBuilder(args);

// --- СЕКЦИЯ СЕРВИСОВ (До builder.Build()) ---

// Добавляем поддержку контроллеров (без этого маршруты не создадутся)
builder.Services.AddControllers();

// Твоя настройка CORS
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

// Исправляем твою ошибку со скрина: добавляем службу авторизации
builder.Services.AddAuthorization();

// Добавляем Swagger (если он еще не добавлен)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- СЕКЦИЯ MIDDLEWARE (После builder.Build()) ---

// Включаем Swagger в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // Нужно, чтобы открывались фото из wwwroot
app.UseCors("AllowAll");
app.UseAuthorization();

// КРИТИЧЕСКИ ВАЖНО: без этой строки API не увидит твой PhotoController
app.MapControllers();

app.Run();