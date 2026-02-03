var builder = WebApplication.CreateBuilder(args);

// 1. Добавь это в секцию сервисов
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// 2. Добавь это ПЕРЕД UseAuthorization
app.UseCors("AllowAll");

app.UseAuthorization();