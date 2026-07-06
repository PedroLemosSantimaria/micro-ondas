using Microondas.Api.Middlewares;
using Microondas.Core.Interfaces;
using Microondas.Core.Services;
using Microondas.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHeatingProgramRepository, HeatingProgramRepository>();
builder.Services.AddSingleton<IMicrowaveService, MicrowaveService>();
builder.Services.AddSingleton<IProgramService, ProgramService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();