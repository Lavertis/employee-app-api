using EmployeeApp.API.Config;
using EmployeeApp.API.CQRS;
using EmployeeApp.API.Middleware;
using EmployeeApp.API.Validators;
using EmployeeApp.Infrastructure.Database;
using EmployeeApp.Infrastructure.Database.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabaseModule(builder.Configuration);
builder.Services.AddMiddlewareModule();
builder.Services.AddMediatorModule();
builder.Services.AddFluentValidators();

var app = builder.Build();
await app.SeedDatabase();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCorsModule();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();