using FluentValidation;
using FootballLeague.API.Endpoints;
using FootballLeague.API.Exceptions;
using FootballLeague.Core.Contracts;
using FootballLeague.Core.Services;
using FootballLeague.Infrastructure.Data;
using FootballLeague.Infrastructure.Persistence;
using FootballLeague.Shared.Constants;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(GlobalConstants.DefaultConnectionString);

builder.Services.AddDbContext<FootballLeagueDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<ITeamStatisticsService, TeamStatisticsService>();
builder.Services.AddScoped<IRankingService, RankingService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.MapTeamEndpoints();
app.MapMatchEndpoints();
app.MapRankingEndpoints();

app.Run();
