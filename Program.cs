using Microsoft.EntityFrameworkCore;

using Cloud5.Data;
using Cloud5.Queries;
using Cloud5.Services;
using Cloud5.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<AppDbContext>(o => o.UseInMemoryDatabase("Cloud5Db"));
builder.Services.AddHostedService<PlayerProcessor>();
builder.Services.AddScoped<GetPlayerStats>();

var app = builder.Build();

StatsEndpoints.Map(app);
app.Run();
