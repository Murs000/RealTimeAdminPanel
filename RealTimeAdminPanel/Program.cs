using Microsoft.OpenApi.Models;
using Quartz;
using RealTimeAdminPanel.Hubs;
using RealTimeAdminPanel.Jobs;
using RealTimeAdminPanel.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddSingleton<StatisticsService>();

// Add Quartz.NET services
builder.Services.AddQuartz(q =>
{
    // Register the job and schedule it to run every 10 seconds
    var jobKey = new JobKey("StatisticsUpdateJob");
    q.AddJob<StatisticsUpdateJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey) // Link the job with this trigger
        .WithIdentity("StatisticsUpdateJob-trigger") // Give the trigger an ID
        .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())); // Trigger every 10 seconds
});

// Add Quartz hosted service
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


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