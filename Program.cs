using GameAPI.Data;
using GameAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// enable CORS for React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .SetIsOriginAllowed(origin =>
                origin
                //.StartsWith("http://frontend-oceanofgames.dev.localhost"))
            .StartsWith("http://localhost:5173"))
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
// SQL Server
builder.Services.AddDbContext<GameDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration
            .GetConnectionString("GameDatabase"));
});


// GameBrain API
builder.Services.AddHttpClient<GameBrainService>(client =>
{
    client.BaseAddress =
        new Uri("https://api.gamebrain.co/v1/");
});

var app = builder.Build();

app.UseCors("FrontendPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
//app.UseStaticFiles();
//app.UseCors("ReactApp");

app.MapControllers();

app.Run();
