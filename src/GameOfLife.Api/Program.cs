using GameOfLife;
using GameOfLife.Models.Options;
using GameOfLife.Repositories;
using GameOfLife.Services;
using GameOfLife.UseCases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GameOfLifeContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.Configure<SimulationOptions>(builder.Configuration.GetSection("Simulation"));

//Repositories
builder.Services.AddScoped<IGameRepository, GameRepository>();

//Use cases
builder.Services.AddScoped<IGetBoardUseCase, GetBoardUseCase>();
builder.Services.AddScoped<IGetBoardStateUseCase, GetBoardStateUseCase>();
builder.Services.AddScoped<ICreateBoardUseCase, CreateBoardUseCase>();
builder.Services.AddScoped<IDeleteBoardUseCase, DeleteBoardUseCase>();

//Services
builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();