using Motoflex.Api.Extensions;
using Motoflex.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiExceptionHandling();

app.UseAuthorization();
app.MapControllers();
app.Services.ExecuteMigrations();

app.Run();
