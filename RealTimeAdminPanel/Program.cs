using Microsoft.OpenApi.Models;
using RealTimeAdminPanel.Hubs;
using RealTimeAdminPanel.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddSingleton<StatisticsService>();
builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();
app.MapHub<StatisticsHub>("/hubs/statistics");
//HandShake {"protocol":"json","version":1}

app.Run();