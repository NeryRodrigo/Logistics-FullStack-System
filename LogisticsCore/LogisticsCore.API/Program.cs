using LogisticsCore.API.Middlewares;
using LogisticsCore.Application;
using LogisticsCore.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// --- 3. AQUÍ CONECTAMOS TUS CAPAS ---
// Registramos MediatR y CQRS
builder.Services.AddApplicationServices();

// Registramos Entity Framework y Repositorios (Pasamos Configuration para leer el ConnectionString)
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWeb",
        policy => policy
            .AllowAnyOrigin() // En producción pondrías el dominio real
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowWeb");

app.UseAuthorization();

app.MapControllers();

app.Run();
