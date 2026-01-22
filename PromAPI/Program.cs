var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // 🔹 moet hier staan
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// 2️⃣ Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();        // 🔹 maakt /swagger beschikbaar
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ⚡ Zorg dat StaticFiles vóór MapControllers
app.UseStaticFiles();

// ⚡ CORS moet vóór MapControllers
app.UseCors("BlazorClient");

app.UseAuthorization();

app.MapControllers();

app.Run();
