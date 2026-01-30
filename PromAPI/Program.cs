var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Упрощенный CORS: разрешаем всё всем
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// 2️⃣ Middleware pipeline
// ВЫНЕСЛИ Swagger из if, чтобы он работал на сервере!
app.UseSwagger();
app.UseSwaggerUI();

// ⚠️ На сервере Ubuntu лучше закомментировать UseHttpsRedirection, 
// если ты еще не настроил SSL сертификаты, иначе запросы могут блокироваться
// app.UseHttpsRedirection();

app.UseStaticFiles();

// Используем политику по умолчанию
app.UseCors();

app.UseAuthorization();
app.MapControllers();

app.Run();